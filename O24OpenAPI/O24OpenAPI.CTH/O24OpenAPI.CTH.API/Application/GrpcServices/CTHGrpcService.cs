using Grpc.Core;
using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Features.UserCommands;
using O24OpenAPI.CTH.API.Application.Features.UserDevice;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.CTH;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.Logging.Helpers;
using static O24OpenAPI.Grpc.CTH.CTHGrpcService;

namespace O24OpenAPI.CTH.API.Application.GrpcServices;

public class CTHGrpcService : CTHGrpcServiceBase
{
    private readonly IUserSessionRepository _userSessionRepository =
        EngineContext.Current.Resolve<IUserSessionRepository>();
    private readonly IUserRightRepository _userRightRepository =
        EngineContext.Current.Resolve<IUserRightRepository>();
    private readonly IUserRightChannelRepository _userRightChannelRepository =
        EngineContext.Current.Resolve<IUserRightChannelRepository>();
    private readonly IUserDeviceRepository _userDeviceRepository =
        EngineContext.Current.Resolve<IUserDeviceRepository>();
    private readonly IUserCommandRepository _userCommandRepository =
        EngineContext.Current.Resolve<IUserCommandRepository>();

    private readonly IUserAccountRepository _userAccountRepository =
        EngineContext.Current.Resolve<IUserAccountRepository>();
    private readonly IUserInRoleRepository userInRoleRepository =
        EngineContext.Current.Resolve<IUserInRoleRepository>();

    private readonly IMediator _mediator = EngineContext.Current.Resolve<IMediator>("cth");

    public override async Task<GrpcResponse> GetUserSession(
        GetUserSessionRequest request,
        ServerCallContext context
    )
    {
        return await _userSessionRepository.GetByToken(request.Token).GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetChannelRoles(
        GetChannelRolesRequest request,
        ServerCallContext context
    )
    {
        return await _userRightChannelRepository
            .GetSetChannelInRoleAsync(request.RoleId)
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetUserPushId(
        GetUserPushIdRequest request,
        ServerCallContext context
    )
    {
        return await GetUserPushIdAsync(request).GetGrpcResponseAsync();
    }

    private async Task<string> GetUserPushIdAsync(GetUserPushIdRequest request)
    {
        UserDevice userAccount = await _userDeviceRepository.GetByUserCodeAsync(request.UserCode);
        string pushId = userAccount?.PushId ?? string.Empty;
        return pushId ?? "NONE";
    }

    public override async Task<GrpcResponse> LoadUserCommands(
        LoadUserCommandsRequest request,
        ServerCallContext context
    )
    {
        return await _userCommandRepository
            .LoadUserCommand(request.ApplicationCode, request.RoleCommand)
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetInfoFromFormCode(
        GetInfoFromFormCodeRequest request,
        ServerCallContext context
    )
    {
        return await _userCommandRepository
            .GetInfoFromFormCode(request.ApplicationCode, request.FormCode)
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetListRoleByUserCode(
        GetListRoleByUserCodeRequest request,
        ServerCallContext context
    )
    {
        return await userInRoleRepository
            .GetListRoleByUserCodeAsync(request.UserCode)
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetInfoFromCommandId(
        GetInfoFromCommandIdRequest request,
        ServerCallContext context
    )
    {
        return await _mediator
            .QueryAsync(
                new GetUserCommandInfoFromCommandIdQuery
                {
                    ApplicationCode = request.ApplicationCode,
                    CommandId = request.CommandId,
                }
            )
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetInfoFromParentId(
        GetInfoFromParentIdRequest request,
        ServerCallContext context
    )
    {
        return await _mediator
            .QueryAsync(
                new GetUserCommandInfoFromParentIdQuery
                {
                    ApplicationCode = request.ApplicationCode,
                    ParentId = request.ParentId,
                }
            )
            .GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetUserInfoByUserCode(
        GetUserInfoByUserCodeRequest request,
        ServerCallContext context
    )
    {
        return await _userAccountRepository
            .GetByUserCodeAsync(request.UserCode)
            .GetGrpcResponseAsync();
    }

    // public override async Task<GrpcResponse> GetUserPushIdByContractNumber(
    //     GetUserPushIdByContractNumberRequest request,
    //     ServerCallContext context
    // )
    // {
    //     return await GrpcExecutor.ExecuteAsync(
    //         context,
    //         async () =>
    //         {
    //             return await _userAccountService.GetUserPushIdByContractNumberAsync(
    //                 request.ContractNumber
    //             );
    //         }
    //     );
    // }

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
        List<CTHUserNotificationModel> list = await _mediator.QueryAsync(
            new GetMobileDeviceQuery()
        );

        foreach (CTHUserNotificationModel item in list)
        {
            GetUserNotificationReply reply = item.ToGetUserNotificationReply();
            await responseStream.WriteAsync(reply!);
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
            IPagedList<CTHUserCommandModel> list = await _mediator.QueryAsync(
                new LoadFullUserCommandsQuery()
            );

            foreach (CTHUserCommandModel item in list)
            {
                try
                {
                    UserCommandReply reply = item.ToUserCommandReply();
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
            BusinessLogHelper.Error(ex, "Error in LoadFullUserCommands GRPC Stream Handler");

            await ex.LogErrorAsync();
        }
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        await Task.CompletedTask;
        return new HelloReply { Hello = $"hello {request.Name}" };
    }
}
