using Newtonsoft.Json.Linq;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Portal;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services.Portal;

public class RoleProfileService(
    IUserCommandService userCommandService,
    IUserRightService userRightService,
    IUserInRoleService userInRoleService
) : IRoleProfileService
{
    private readonly JWebUIObjectContextModel context =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();
    private readonly IUserCommandService _userCommandService = userCommandService;
    private readonly IUserRightService _userRightService = userRightService;
    private readonly IUserInRoleService _userInRoleService = userInRoleService;

    public async Task<string> LoadRoleOperation()
    {
        var boInput = context.Bo.GetBoInput();
        if (boInput["command_id"] != null && boInput["channel_id"] != null)
        {
            var command_id = boInput["command_id"].ToString();
            var app = boInput["channel_id"].ToString();
            var getOperationData = await _userCommandService.GetUserCommandInfoFromParentId(
                app,
                command_id
            );
            var getCommandsFromCommand = await _userCommandService.GetUserCommandInfoFromCommandId(
                app,
                command_id
            );
            getCommandsFromCommand.AddRange(getOperationData);
            getCommandsFromCommand = getCommandsFromCommand.OrderBy(x => x.RoleId).ToList();
            if (getCommandsFromCommand != null)
            {
                var operationHeader = await getCommandsFromCommand
                    .FindAll(s => s.ParentId.Equals(command_id) && s.CommandType.Equals("C"))
                    .Select(s => new OperationHeaderModel()
                    {
                        cmdid = s.CommandId,
                        caption = s.CommandName,
                    })
                    .ToListAsync();

                var operationBody = getCommandsFromCommand.FindAll(s =>
                    s.CommandId.Equals(command_id) || s.CommandType.Equals("C")
                );

                JObject dataRs = new JObject
                {
                    ["operation_header"] = operationHeader.DistinctBy(p => p.cmdid).ToJArray(),
                    ["operation_body"] = operationBody.ToJArray(),
                };
                context.Bo.AddPackFo("data", dataRs);
                return "true";
            }
        }
        return "false";
    }

    public async Task<string> LoadMenuByChannel(ModelWithChannel model)
    {
        var commandMenus = await _userCommandService.GetCommandMenuByChannel(model.ChannelId);
        context.Bo.AddPackFo("data", commandMenus);
        return "true";
    }

    public async Task UpdateUserRight(UserRightUpdateModel model)
    {
        foreach (var item in model.ListUserRight)
        {
            var entity = await _userRightService.GetByRoleIdAndCommandIdAsync(
                item.RoleId,
                item.CommandId
            );
            if (entity != null)
            {
                entity = item.ToEntity(entity);
                await _userRightService.UpdateAsync(entity);
            }
            else
            {
                entity = item.FromModel<UserRight>();
                await _userRightService.AddAsync(entity);
            }
        }
    }

    public async Task UpdateUserInRole(UpdateUserInRoleModel model)
    {
        var existingUsers = await _userInRoleService.GetListUserByRoleId(model.RoleId);
        var existingUserDict = existingUsers.ToDictionary(u => u.UserCode);

        List<Task> tasks = [];

        foreach (var userCode in model.ListUser)
        {
            existingUserDict.TryGetValue(userCode, out var entity);

            if (model.Action == ActionUpdateUserInRole.Remove)
            {
                if (entity != null)
                {
                    tasks.Add(_userInRoleService.Delete(entity));
                }
                else
                {
                    throw new O24OpenAPIException(
                        $"User [{userCode}] does not exist in role [{model.RoleId}]"
                    );
                }
            }
            else
            {
                if (entity == null)
                {
                    var newUserInRole = new UserInRole()
                    {
                        RoleId = model.RoleId,
                        UserCode = userCode,
                    };
                    tasks.Add(_userInRoleService.Create(newUserInRole));
                }
                else
                {
                    throw new O24OpenAPIException(
                        $"User [{userCode}] already exists in role [{model.RoleId}]"
                    );
                }
            }
        }

        await Task.WhenAll(tasks);
    }
}
