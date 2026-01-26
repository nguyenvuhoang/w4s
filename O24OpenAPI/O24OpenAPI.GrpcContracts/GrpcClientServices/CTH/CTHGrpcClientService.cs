using Grpc.Core;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.CTH;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;

public class CTHGrpcClientService : BaseGrpcClientService, ICTHGrpcClientService
{
    private readonly IGrpcClientFactory _grpcClientFactory;
    private readonly Metadata _defaultHeader;

    public CTHGrpcClientService(IGrpcClientFactory grpcClientFactory)
    {
        ServerId = "CTH";
        _grpcClientFactory = grpcClientFactory;
        _defaultHeader = new Metadata()
        {
            {
                "flow",
                $"{Singleton<O24OpenAPIConfiguration>.Instance?.YourServiceID} -> {ServerId}"
            },
        };
    }

    public async Task<CTHUserSessionModel> GetUserSessionAsync(string token)
    {
        GetUserSessionRequest request = new() { Token = token };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetUserSessionAsync(request, _defaultHeader)
            .CallAsync<CTHUserSessionModel>();
    }

    public async Task<HashSet<string>> GetChannelRolesAsync(int roleId)
    {
        GetChannelRolesRequest request = new() { RoleId = roleId };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetChannelRolesAsync(request, _defaultHeader)
            .CallAsync<HashSet<string>>();
    }

    public async Task<string> GetUserPushIdAsync(string userCode)
    {
        GetUserPushIdRequest request = new() { UserCode = userCode };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient.GetUserPushIdAsync(request, _defaultHeader).CallAsync<string>();
    }

    public async Task<List<CTHUserCommandModel>> LoadUserCommandsAsync(
        string applicationCode,
        string roleCommand
    )
    {
        LoadUserCommandsRequest request = new()
        {
            ApplicationCode = applicationCode,
            RoleCommand = roleCommand,
        };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .LoadUserCommandsAsync(request, _defaultHeader)
            .CallAsync<List<CTHUserCommandModel>>();
    }

    public async Task<List<CTHUserCommandModel>> GetInfoFromFormCodeAsync(
        string applicationCode,
        string formCode
    )
    {
        GetInfoFromFormCodeRequest request = new()
        {
            ApplicationCode = applicationCode,
            FormCode = formCode,
        };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetInfoFromFormCodeAsync(request)
            .CallAsync<List<CTHUserCommandModel>>();
    }

    public async Task<List<CTHUserInRoleModel>> GetListRoleByUserCodeAsync(string userCode)
    {
        GetListRoleByUserCodeRequest request = new() { UserCode = userCode };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetListRoleByUserCodeAsync(request, _defaultHeader)
            .CallAsync<List<CTHUserInRoleModel>>();
    }

    public async Task<List<CTHCommandIdInfoModel>> GetInfoFromCommandIdAsync(
        string applicationCode,
        string commandId
    )
    {
        GetInfoFromCommandIdRequest request = new()
        {
            ApplicationCode = applicationCode,
            CommandId = commandId,
        };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetInfoFromCommandIdAsync(request, _defaultHeader)
            .CallAsync<List<CTHCommandIdInfoModel>>();
    }

    public async Task<List<CTHCommandIdInfoModel>> GetInfoFromParentIdAsync(
        string applicationCode,
        string parentId
    )
    {
        GetInfoFromParentIdRequest request = new()
        {
            ApplicationCode = applicationCode,
            ParentId = parentId,
        };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetInfoFromParentIdAsync(request, _defaultHeader)
            .CallAsync<List<CTHCommandIdInfoModel>>();
    }

    public async Task<CTHUserAccountModel> GetUserInfoByUserCodeAsync(string userCode)
    {
        GetUserInfoByUserCodeRequest request = new() { UserCode = userCode };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetUserInfoByUserCodeAsync(request, _defaultHeader)
            .CallAsync<CTHUserAccountModel>();
    }

    public async Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber)
    {
        GetUserPushIdByContractNumberRequest request = new() { ContractNumber = contractNumber };
        CTHGrpcService.CTHGrpcServiceClient cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetUserPushIdByContractNumberAsync(request, _defaultHeader)
            .CallAsync<CTHUserPushModel>();
    }

    /// <summary>
    /// Streaming gRPC to get user notifications
    /// </summary>
    /// <returns></returns>
    public async Task<List<CTHUserNotificationModel?>> GetUserNotificationAsync()
    {
        GetUserNotificationRequest request = new() { };
        Task<AsyncServerStreamingCall<GetUserNotificationReply>> streamClient = _grpcClientFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            GetUserNotificationRequest,
            GetUserNotificationReply
        >((client, request, option) => client.GetUserNotification(request, option), request);

        using AsyncServerStreamingCall<GetUserNotificationReply> streamingCall = await streamClient;
        List<CTHUserNotificationModel?> response = [];
        await foreach (GetUserNotificationReply? reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply?.ToCTHUserNotificationModel());
        }
        return response;
    }

    /// <summary>
    /// Load full user commands via streaming gRPC
    /// </summary>
    /// <returns></returns>
    public async Task<List<CTHUserCommandModel?>> LoadFullUserCommandAsync()
    {
        LoadFullUserCommandRequest request = new() { };
        Task<AsyncServerStreamingCall<UserCommandReply>> streamClient = _grpcClientFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            LoadFullUserCommandRequest,
            UserCommandReply
        >((client, request, option) => client.LoadFullUserCommands(request, option), request);

        using AsyncServerStreamingCall<UserCommandReply> streamingCall = await streamClient;
        List<CTHUserCommandModel?> response = [];
        await foreach (UserCommandReply? reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply?.ToCTHUserCommandModel());
        }
        return response;
    }
}
