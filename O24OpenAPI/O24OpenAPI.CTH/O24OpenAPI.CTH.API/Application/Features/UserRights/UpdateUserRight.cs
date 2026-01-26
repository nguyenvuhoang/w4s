using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.UserRights;

public class UpdateUserRightCommand : BaseTransactionModel, ICommand<bool>
{
    public List<UserRightModel> ListUserRight { get; set; }
}

[CqrsHandler]
public class UpdateUserRightHandle(
    IUserCommandRepository userCommandRepository,
    IUserRightRepository userRightRepository,
    IUserRoleRepository userRoleRepository
) : ICommandHandler<UpdateUserRightCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_UPDATE_RIGHT)]
    public async Task<bool> HandleAsync(
        UpdateUserRightCommand request,
        CancellationToken cancellationToken = default
    )
    {
        foreach (UserRightModel item in request.ListUserRight)
        {
            List<UserCommandResponseModel> getInfoFromCommandId =
                await GetInfoFromCommandId(request.ChannelId, item.CommandId) ?? [];

            if (getInfoFromCommandId.Count > 0)
            {
                UserCommandResponseModel getCommand = getInfoFromCommandId
                    .Where(s => s.CommandId == item.CommandId)
                    .FirstOrDefault();
                if (getCommand != null)
                {
                    UserRight parentRight = await userRightRepository.GetByRoleIdAndCommandIdAsync(
                        item.RoleId,
                        getCommand.ParentId
                    );

                    if (parentRight == null)
                    {
                        UserRight newUserRight = new()
                        {
                            RoleId = item.RoleId,
                            CommandId = getCommand.ParentId,
                            CommandIdDetail = "A",
                            Invoke = 1,
                            Approve = 1,
                            CreatedOnUtc = DateTime.UtcNow,
                            UpdatedOnUtc = DateTime.UtcNow,
                        };
                        await userRightRepository.AddUserRightAsync(newUserRight);
                    }
                }
            }

            UserRight entity = await userRightRepository.GetByRoleIdAndCommandIdAsync(
                item.RoleId,
                item.CommandId
            );
            if (entity != null)
            {
                entity = item.ToEntity(entity);
                await userRightRepository.UpdateAsync(entity);
            }
            else
            {
                entity = item.FromModel<UserRight>();
                await userRightRepository.AddUserRightAsync(entity);
            }
        }
        return true;
    }

    public virtual async Task<List<UserCommandResponseModel>> GetInfoFromCommandId(
        string applicationCode,
        string commandId
    )
    {
        List<UserCommandResponseModel> listLeftJoin = await (
            from userCommand in userCommandRepository.Table
            join userRight in userRightRepository.Table
                on userCommand.CommandId equals userRight.CommandId
            join userRole in userRoleRepository.Table on userRight.RoleId equals userRole.RoleId
            where
                userCommand.ApplicationCode == applicationCode
                && userCommand.CommandId == commandId
                && userCommand.Enabled
            select new UserCommandResponseModel
            {
                ParentId = userCommand.ParentId,
                CommandId = userCommand.CommandId,
                CommandNameLanguage = userCommand.CommandName,
                Icon = userCommand.GroupMenuIcon,
                GroupMenuVisible = userCommand.GroupMenuVisible,
                GroupMenuId = userCommand.GroupMenuId,
            }
        ).ToListAsync();

        return listLeftJoin;
    }
}
