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
                $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}"
            },
        };
    }

    public async Task<CTHUserSessionModel> GetUserSessionAsync(string token)
    {
        var request = new GetUserSessionRequest { Token = token };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetUserSessionAsync(request, _defaultHeader)
            .CallAsync<CTHUserSessionModel>();
    }

    public async Task<HashSet<string>> GetChannelRolesAsync(int roleId)
    {
        var request = new GetChannelRolesRequest { RoleId = roleId };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetChannelRolesAsync(request, _defaultHeader)
            .CallAsync<HashSet<string>>();
    }

    public async Task<string> GetUserPushIdAsync(string userCode)
    {
        var request = new GetUserPushIdRequest { UserCode = userCode };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient.GetUserPushIdAsync(request, _defaultHeader).CallAsync<string>();
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
        var cthGrpcClient =
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
        var request = new GetInfoFromFormCodeRequest
        {
            ApplicationCode = applicationCode,
            FormCode = formCode,
        };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetInfoFromFormCodeAsync(request)
            .CallAsync<List<CTHUserCommandModel>>();
    }

    public async Task<List<CTHUserInRoleModel>> GetListRoleByUserCodeAsync(string userCode)
    {
        var request = new GetListRoleByUserCodeRequest { UserCode = userCode };
        var cthGrpcClient =
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
        var request = new GetInfoFromCommandIdRequest
        {
            ApplicationCode = applicationCode,
            CommandId = commandId,
        };
        var cthGrpcClient =
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
        var request = new GetInfoFromParentIdRequest
        {
            ApplicationCode = applicationCode,
            ParentId = parentId,
        };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetInfoFromParentIdAsync(request, _defaultHeader)
            .CallAsync<List<CTHCommandIdInfoModel>>();
    }

    public async Task<CTHUserAccountModel> GetUserInfoByUserCodeAsync(string userCode)
    {
        var request = new GetUserInfoByUserCodeRequest { UserCode = userCode };
        var cthGrpcClient =
            await _grpcClientFactory.GetClientAsync<CTHGrpcService.CTHGrpcServiceClient>();
        return await cthGrpcClient
            .GetUserInfoByUserCodeAsync(request, _defaultHeader)
            .CallAsync<CTHUserAccountModel>();
    }

    public async Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber)
    {
        var request = new GetUserPushIdByContractNumberRequest { ContractNumber = contractNumber };
        var cthGrpcClient =
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
        var request = new GetUserNotificationRequest { };
        var streamClient = _grpcClientFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            GetUserNotificationRequest,
            GetUserNotificationReply
        >((client, request, option) => client.GetUserNotification(request, option), request);

        using var streamingCall = await streamClient;
        var response = new List<CTHUserNotificationModel?>();
        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
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
        var request = new LoadFullUserCommandRequest { };
        var streamClient = _grpcClientFactory.GetServerStreamAsync<
            CTHGrpcService.CTHGrpcServiceClient,
            LoadFullUserCommandRequest,
            UserCommandReply
        >((client, request, option) => client.LoadFullUserCommands(request, option), request);

        using var streamingCall = await streamClient;
        var response = new List<CTHUserCommandModel?>();
        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply?.ToCTHUserCommandModel());
        }
        return response;
    }
}
