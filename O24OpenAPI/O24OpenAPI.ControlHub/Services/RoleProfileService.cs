using Newtonsoft.Json.Linq;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.ControlHub.Services;

public class RoleProfileService(
    IUserCommandService userCommandService,
    IUserRightService userRightService
) : IRoleProfileService
{
    private readonly IUserCommandService _userCommandService = userCommandService;
    private readonly IUserRightService _userRightService = userRightService;

    /// <summary>
    /// LoadRoleOperationAsync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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

        var getOperationData = await _userCommandService.GetUserCommandInfoFromParentId(
            app,
            command_id
        );
        var getCommandsFromCommand =
            await _userCommandService.GetUserCommandInfoFromCommandId(app, command_id) ?? [];

        getCommandsFromCommand.AddRange(getOperationData ?? []);
        getCommandsFromCommand = [.. getCommandsFromCommand.OrderBy(x => x.RoleId)];

        var operationHeader = getCommandsFromCommand
            .Where(s => s.ParentId == command_id && s.CommandType == "C")
            .Select(s => new OperationHeaderModel { cmdid = s.CommandId, caption = s.CommandName })
            .DistinctBy(p => p.cmdid)
            .ToList();

        var operationBody = getCommandsFromCommand
            .Where(s => s.CommandId == command_id || s.CommandType == "C")
            .ToList();

        result["operation_header"] = JArray.FromObject(operationHeader);
        result["operation_body"] = JArray.FromObject(operationBody);

        return result;
    }

    /// <summary>
    /// LoadMenuByChannelAsync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<List<UserCommandResponseModel>> LoadMenuByChannelAsync(
        UserCommandRequestModel model
    )
    {
        var commandMenus = await _userCommandService.GetCommandMenuByChannel(model.ChannelId);

        return [.. commandMenus.OrderBy(x => x.DisplayOrder)];
    }

    /// <summary>
    /// UpdateUserRight
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateUserRightAsync(UserRightUpdateModel model)
    {
        foreach (var item in model.ListUserRight)
        {
            var getInfoFromCommandId =
                await _userCommandService.GetInfoFromCommandId(model.ChannelId, item.CommandId)
                ?? [];

            if (getInfoFromCommandId.Count > 0)
            {
                var getCommand = getInfoFromCommandId
                    .Where(s => s.CommandId == item.CommandId)
                    .FirstOrDefault();
                if (getCommand != null)
                {
                    var parentRight = await _userRightService.GetByRoleIdAndCommandIdAsync(
                        item.RoleId,
                        getCommand.ParentId
                    );

                    if (parentRight == null)
                    {
                        var newUserRight = new UserRight
                        {
                            RoleId = item.RoleId,
                            CommandId = getCommand.ParentId,
                            CommandIdDetail = "A",
                            Invoke = 1,
                            Approve = 1,
                            CreatedOnUtc = DateTime.UtcNow,
                            UpdatedOnUtc = DateTime.UtcNow,
                        };
                        await _userRightService.AddUserRightAsync(newUserRight);
                    }
                }
            }

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
                await _userRightService.AddUserRightAsync(entity);
            }
        }
        return true;
    }
}
