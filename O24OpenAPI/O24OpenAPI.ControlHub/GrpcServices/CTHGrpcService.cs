using Grpc.Core;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.CTH;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.Web.Framework.Extensions;
using static O24OpenAPI.Grpc.CTH.CTHGrpcService;

namespace O24OpenAPI.ControlHub.GrpcServices;

/// <summary>
/// The admin grpc service class
/// </summary>
public class CTHGrpcService : CTHGrpcServiceBase
{
    private readonly IUserSessionService _userSessionService =
        EngineContext.Current.Resolve<IUserSessionService>();
    private readonly IUserRightService _userRightService =
        EngineContext.Current.Resolve<IUserRightService>();
    private readonly IUserDeviceService _userDeviceService =
        EngineContext.Current.Resolve<IUserDeviceService>();
    private readonly IUserCommandService _userCommandService =
        EngineContext.Current.Resolve<IUserCommandService>();
    private readonly IUserInRoleService _userInRoleService =
        EngineContext.Current.Resolve<IUserInRoleService>();
    private readonly IUserAccountService _userAccountService =
        EngineContext.Current.Resolve<IUserAccountService>();

    public override async Task<GrpcResponse> GetUserSession(
        GetUserSessionRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userSessionService.GetByToken(request.Token);
            }
        );
    }

    public override async Task<GrpcResponse> GetChannelRoles(
        GetChannelRolesRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userRightService.GetSetChannelInRoleAsync(request.RoleId);
            }
        );
    }

    public override async Task<GrpcResponse> GetUserPushId(
        GetUserPushIdRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var userAccount = await _userDeviceService.GetByUserCodeAsync(request.UserCode);
                var pushId = userAccount?.PushId ?? string.Empty;
                return pushId ?? "NONE";
            }
        );
    }

    public override async Task<GrpcResponse> LoadUserCommands(
        LoadUserCommandsRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userCommandService.LoadUserCommand(
                    request.ApplicationCode,
                    request.RoleCommand
                );
            }
        );
    }

    public override async Task<GrpcResponse> GetInfoFromFormCode(
        GetInfoFromFormCodeRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userCommandService.GetInfoFromFormCode(
                    request.ApplicationCode,
                    request.FormCode
                );
            }
        );
    }

    public override async Task<GrpcResponse> GetListRoleByUserCode(
        GetListRoleByUserCodeRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userInRoleService.GetListRoleByUserCodeAsync(request.UserCode);
            }
        );
    }

    public override async Task<GrpcResponse> GetInfoFromCommandId(
        GetInfoFromCommandIdRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userCommandService.GetUserCommandInfoFromCommandId(
                    request.ApplicationCode,
                    request.CommandId
                );
            }
        );
    }

    public override async Task<GrpcResponse> GetInfoFromParentId(
        GetInfoFromParentIdRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userCommandService.GetUserCommandInfoFromParentId(
                    request.ApplicationCode,
                    request.ParentId
                );
            }
        );
    }

    public override async Task<GrpcResponse> GetUserInfoByUserCode(
        GetUserInfoByUserCodeRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userAccountService.GetByUserCodeAsync(
                    request.UserCode
                );
            }
        );
    }

    public override async Task<GrpcResponse> GetUserPushIdByContractNumber(
        GetUserPushIdByContractNumberRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return await _userAccountService.GetUserPushIdByContractNumberAsync(
                    request.ContractNumber
                );
            }
        );
    }


    /// <summary>
    /// Get user notification stream
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <returns></returns>

    public override async Task GetUserNotification(
        GetUserNotificationRequest request,
        IServerStreamWriter<GetUserNotificationReply> responseStream,
        ServerCallContext context
    )
    {
        var list = await _userDeviceService.GetMobileDevice();

        foreach (var item in list)
        {
            var reply = item.ToGetUserNotificationReply();
            await responseStream.WriteAsync(reply);
        }
    }

    /// <summary>
    /// Load full user commands
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task LoadFullUserCommands(
        LoadFullUserCommandRequest request,
        IServerStreamWriter<UserCommandReply> responseStream,
        ServerCallContext context
    )
    {
        try
        {
            var list = await _userCommandService.LoadFullUserCommandsAsync();

            foreach (var item in list)
            {
                try
                {
                    var reply = item.ToUserCommandReply();
                    await responseStream.WriteAsync(reply);
                }
                catch (Exception exItem)
                {
                    BusinessLogHelper.Error(
                        exItem,
                        "Error mapping UserCommand to UserCommandReply. CommandId={CommandId}, ApplicationCode={ApplicationCode}",
                        item?.CommandId,
                        item?.ApplicationCode
                    );

                    await exItem.LogErrorAsync();
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogHelper.Error(
                ex,
                "Error in LoadFullUserCommands GRPC Stream Handler"
            );

            await ex.LogErrorAsync();
        }
    }


}
