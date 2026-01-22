using Grpc.Core;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;

public class WFOGrpcClientService : BaseGrpcClientService, IWFOGrpcClientService
{
    private readonly IGrpcClientFactory _grpcClientFactory;
    private readonly Metadata _defaultHeader;

    public WFOGrpcClientService(IGrpcClientFactory grpcClientFactory)
    {
        ServerId = "WFO";
        _grpcClientFactory = grpcClientFactory;
        _defaultHeader = new Metadata()
        {
            {
                "flow",
                $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}"
            },
        };
    }

    public async Task<string> ExecuteWorkflowAsync(string workflowInputJson)
    {
        ExecuteWorkflowRequest request = new() { WorkflowInputJson = workflowInputJson };
        WFOGrpcService.WFOGrpcServiceClient wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient
            .ExecuteWorkflowAsync(request, _defaultHeader)
            .CallAsync<string>();
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new O24OpenAPIException("ExecuteWorkflowAsync return null.");
        }
        return result;
    }

    public async Task<string> RegisterServiceGrpcEndpointAsync(
        string serviceCode,
        string serviceHandleName,
        string grpcEndpointURL,
        string instanceID,
        string serviceAssemblyName
    )
    {
        RegisterServiceGrpcEndpointRequest request = new()
        {
            ServiceCode = serviceCode,
            ServiceHandleName = serviceHandleName,
            GrpcEndpointUrl = grpcEndpointURL,
            InstanceId = instanceID,
            ServiceAssemblyName = serviceAssemblyName,
        };
        WFOGrpcService.WFOGrpcServiceClient wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient
            .RegisterServiceGrpcEndpointAsync(request, _defaultHeader)
            .CallAsync<string>();
        return result;
    }

    public async Task<ServiceInfo> QueryServiceInfoAsync(
        string fromServiceCode,
        string toServiceCode,
        string instanceID
    )
    {
        QueryServiceInfoRequest request = new()
        {
            FromServiceCode = fromServiceCode,
            ToServiceCode = toServiceCode,
            InstanceId = instanceID,
        };
        WFOGrpcService.WFOGrpcServiceClient wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        ServiceInfo result = await wfoGrpcClient
            .QueryServiceInfoAsync(request, _defaultHeader)
            .CallAsync<ServiceInfo>();
        return result;
    }

    public async Task<ServiceInfo> GetServiceInstanceByServiceHandleNameAsync(
        string serviceHandleName
    )
    {
        GetServiceInstanceByServiceHandleNameRequest request = new()
        {
            ServiceHandleName = serviceHandleName,
        };
        WFOGrpcService.WFOGrpcServiceClient wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        ServiceInfo result = await wfoGrpcClient
            .GetServiceInstanceByServiceHandleNameAsync(request, _defaultHeader)
            .CallAsync<ServiceInfo>();
        return result;
    }

    /// <summary>
    /// Ping Server
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<string> PingAsync(string name)
    {
        HelloRequest request = new() { Name = name };
        WFOGrpcService.WFOGrpcServiceClient wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient.PingAsync(request, _defaultHeader).CallAsync<string>();
        return result;
    }
}
