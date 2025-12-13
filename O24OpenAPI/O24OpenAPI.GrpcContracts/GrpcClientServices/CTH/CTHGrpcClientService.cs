using Grpc.Core;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.CTH;
using O24OpenAPI.GrpcContracts.Factory;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;

public class CTHGrpcClientService : BaseGrpcClientService, ICTHGrpcClientService
{
    public CTHGrpcClientService()
    {
        ServerId = "CTH";
    }

    private readonly IGrpcClient<CTHGrpcService.CTHGrpcServiceClient> _cthGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<CTHGrpcService.CTHGrpcServiceClient>>();

    public async Task<CTHUserSessionModel> GetUserSessionAsync(string token)
    {
        var request = new GetUserSessionRequest { Token = token };
        return await InvokeAsync<CTHUserSessionModel>(
            async (header) => await _cthGrpcClient.Client.GetUserSessionAsync(request, header)
        );
    }

    public async Task<HashSet<string>> GetChannelRolesAsync(int roleId)
    {
        var request = new GetChannelRolesRequest { RoleId = roleId };
        return await InvokeAsync<HashSet<string>>(
            async (header) => await _cthGrpcClient.Client.GetChannelRolesAsync(request, header)
        );
    }

    public async Task<string> GetUserPushIdAsync(string userCode)
    {
        var request = new GetUserPushIdRequest { UserCode = userCode };
        return await InvokeAsync<string>(
            async (header) => await _cthGrpcClient.Client.GetUserPushIdAsync(request, header)
        );
    }

    public async Task<List<CTHUserCommandModel>> LoadUserCommandsAsync(
        string applicationCode,
        string roleCommand
    )
    {
        var request = new LoadUserCommandsRequest
        {
            ApplicationCode = applicationCode,
            RoleCommand = roleCommand,
        };
        return await InvokeAsync<List<CTHUserCommandModel>>(
            async (header) => await _cthGrpcClient.Client.LoadUserCommandsAsync(request, header)
        );
    }

    public async Task<List<CTHUserCommandModel>> GetInfoFromFormCodeAsync(
        string applicationCode,
        string formCode
    )
    {
        var request = new GetInfoFromFormCodeRequest
        {
            ApplicationCode = applicationCode,
            FormCode = formCode,
        };
        return await InvokeAsync<List<CTHUserCommandModel>>(
            async (header) => await _cthGrpcClient.Client.GetInfoFromFormCodeAsync(request, header)
        );
    }

    public async Task<List<CTHUserInRoleModel>> GetListRoleByUserCodeAsync(string userCode)
    {
        var request = new GetListRoleByUserCodeRequest { UserCode = userCode };
        return await InvokeAsync<List<CTHUserInRoleModel>>(
            async (header) =>
                await _cthGrpcClient.Client.GetListRoleByUserCodeAsync(request, header)
        );
    }

    public async Task<List<CTHCommandIdInfoModel>> GetInfoFromCommandIdAsync(
        string applicationCode,
        string commandId
    )
    {
        var request = new GetInfoFromCommandIdRequest
        {
            ApplicationCode = applicationCode,
            CommandId = commandId,
        };
        return await InvokeAsync<List<CTHCommandIdInfoModel>>(
            async (header) => await _cthGrpcClient.Client.GetInfoFromCommandIdAsync(request, header)
        );
    }

    public async Task<List<CTHCommandIdInfoModel>> GetInfoFromParentIdAsync(
        string applicationCode,
        string parentId
    )
    {
        var request = new GetInfoFromParentIdRequest
        {
            ApplicationCode = applicationCode,
            ParentId = parentId,
        };
        return await InvokeAsync<List<CTHCommandIdInfoModel>>(
            async (header) => await _cthGrpcClient.Client.GetInfoFromParentIdAsync(request, header)
        );
    }

    public async Task<CTHUserAccountModel> GetUserInfoByUserCodeAsync(string userCode)
    {
        var request = new GetUserInfoByUserCodeRequest
        {
            UserCode = userCode,
        };
        return await InvokeAsync<CTHUserAccountModel>(
            async (header) => await _cthGrpcClient.Client.GetUserInfoByUserCodeAsync(request, header)
        );
    }

    public async Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber)
    {
        var request = new GetUserPushIdByContractNumberRequest
        {
            ContractNumber = contractNumber
        };
        return await InvokeAsync<CTHUserPushModel>(
            async (header) => await _cthGrpcClient.Client.GetUserPushIdByContractNumberAsync(request, header)
        );
    }

    /// <summary>
    /// Streaming gRPC to get user notifications
    /// </summary>
    /// <returns></returns>
    public async Task<List<CTHUserNotificationModel>> GetUserNotificationAsync()
    {
        var grpcFactory = EngineContext.Current.Resolve<IGrpcClientFactory>();
        var request = new GetUserNotificationRequest { };
        var streamClient = grpcFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            GetUserNotificationRequest,
            GetUserNotificationReply>((client, request, option) => client.GetUserNotification(request, option), request);

        using var streamingCall = await streamClient;
        var response = new List<CTHUserNotificationModel>();
        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply.ToCTHUserNotificationModel());
        }
        return response;
    }

    /// <summary>
    /// Load full user commands via streaming gRPC
    /// </summary>
    /// <returns></returns>
    public async Task<List<CTHUserCommandModel>> LoadFullUserCommandAsync()
    {
        var grpcFactory = EngineContext.Current.Resolve<IGrpcClientFactory>();
        var request = new LoadFullUserCommandRequest { };
        var streamClient = grpcFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            LoadFullUserCommandRequest,
            UserCommandReply>((client, request, option) => client.LoadFullUserCommands(request, option), request);

        using var streamingCall = await streamClient;
        var response = new List<CTHUserCommandModel>();
        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply.ToCTHUserCommandModel());
        }
        return response;
    }
}
