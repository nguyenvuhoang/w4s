using LinKit.Core.Cqrs;
using LinqToDB;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Repositories;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class LoadOperationCommand : BaseTransactionModel, ICommand<JObject> { }
    [CqrsHandler]
    public class LoadOperationHandle(
        WFScheme wFScheme,
        IUserCommandRepository userCommandRepository,
        IUserRoleRepository userRoleRepository,
        IUserRightRepository userRightRepository
    ) : ICommandHandler<LoadOperationCommand, JObject>
    {
        [WorkflowStep("WF_STEP_CTH_LOAD_OPERATION")]
        public Task<JObject> HandleAsync(
            LoadOperationCommand request,
            CancellationToken cancellationToken = default
        )
        {
            var model = wFScheme.ToModel<UserCommandRequestModel>();

            var response = LoadRoleOperationAsync(model);
            return response;
        }

        public async Task<JObject> LoadRoleOperationAsync(UserCommandRequestModel model)
        {
            var result = new JObject();

            if (model.CommandId == null || model.ChannelId == null)
            {
                result["error"] = "Missing CommandId or ChannelId";
                return result;
            }

            var command_id = model.CommandId;
            var app = model.ChannelId;

            var getOperationData = await GetUserCommandInfoFromParentId(app, command_id);
            var getCommandsFromCommand =
                await GetUserCommandInfoFromCommandId(app, command_id) ?? [];

            getCommandsFromCommand.AddRange(getOperationData ?? []);
            getCommandsFromCommand = [.. getCommandsFromCommand.OrderBy(x => x.RoleId)];

            var operationHeader = getCommandsFromCommand
                .Where(s => s.ParentId == command_id && s.CommandType == "C")
                .Select(s => new OperationHeaderModel
                {
                    cmdid = s.CommandId,
                    caption = s.CommandName,
                })
                .DistinctBy(p => p.cmdid)
                .ToList();

            var operationBody = getCommandsFromCommand
                .Where(s => s.CommandId == command_id || s.CommandType == "C")
                .ToList();

            result["operation_header"] = JArray.FromObject(operationHeader);
            result["operation_body"] = JArray.FromObject(operationBody);

            return result;
        }

        public virtual async Task<List<CommandIdInfoModel>> GetUserCommandInfoFromParentId(
            string applicationCode,
            string parentId
        )
        {
            var listLeftJoin = await (
                from userCommand in userCommandRepository.Table.Where(s =>
                    s.ApplicationCode == applicationCode && s.ParentId == parentId && s.Enabled
                )
                from userRole in userRoleRepository.Table.DefaultIfEmpty()
                from userRight in userRightRepository
                    .Table.Where(s =>
                        s.CommandId == userCommand.CommandId && s.RoleId == userRole.RoleId
                    )
                    .DefaultIfEmpty()

                select new CommandIdInfoModel()
                {
                    ParentId = userCommand.ParentId,
                    CommandId = userCommand.CommandId,
                    CommandName = userCommand.CommandName,
                    ApplicationCode = userCommand.ApplicationCode,
                    CommandType = userCommand.CommandType,
                    RoleId = userRole.RoleId,
                    RoleName = userRole.RoleName,
                    CommandIdDetail =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.CommandIdDetail
                            : null,
                    Invoke =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.Invoke
                            : 0,
                    Approve =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.Approve
                            : 0,
                    GroupMenuIcon = userCommand.GroupMenuIcon,
                    GroupMenuVisible = userCommand.GroupMenuVisible,
                    GroupMenuId = userCommand.GroupMenuId,
                    GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm,
                }
            )
                .OrderBy(s => s.RoleId)
                .ThenBy(s => s.ParentId)
                .ThenBy(s => s.CommandId)
                .ToListAsync();

            return listLeftJoin;
        }

        public virtual async Task<List<CommandIdInfoModel>> GetUserCommandInfoFromCommandId(
            string applicationCode,
            string commandId
        )
        {
            var listLeftJoin = await (
                from userCommand in userCommandRepository.Table.Where(s =>
                    s.ApplicationCode == applicationCode && s.CommandId == commandId && s.Enabled
                )
                from userRole in userRoleRepository.Table.DefaultIfEmpty()
                from userRight in userRightRepository
                    .Table.Where(s => s.CommandId == commandId && s.RoleId == userRole.RoleId)
                    .DefaultIfEmpty()

                select new CommandIdInfoModel()
                {
                    ParentId = userCommand.ParentId,
                    CommandId = userCommand.CommandId,
                    CommandName = userCommand.CommandName,
                    ApplicationCode = userCommand.ApplicationCode,
                    CommandType = userCommand.CommandType,
                    RoleId = userRole.RoleId,
                    RoleName = userRole.RoleName,
                    CommandIdDetail =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.CommandIdDetail
                            : null,
                    Invoke =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.Invoke
                            : 0,
                    Approve =
                        (
                            userCommand.CommandId == userRight.CommandId
                            && userRole.RoleId == userRight.RoleId
                        )
                            ? userRight.Approve
                            : 0,
                    GroupMenuIcon = userCommand.GroupMenuIcon,
                    GroupMenuVisible = userCommand.GroupMenuVisible,
                    GroupMenuId = userCommand.GroupMenuId,
                    GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm,
                }
            )
                .OrderBy(s => s.RoleId)
                .ThenBy(s => s.ParentId)
                .ThenBy(s => s.CommandId)
                .ToListAsync();

            return listLeftJoin;
        }
    }
}
