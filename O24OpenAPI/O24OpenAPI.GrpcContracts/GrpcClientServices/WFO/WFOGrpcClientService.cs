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
        var request = new ExecuteWorkflowRequest { WorkflowInputJson = workflowInputJson };
        var wfoGrpcClient =
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
        var request = new RegisterServiceGrpcEndpointRequest
        {
            ServiceCode = serviceCode,
            ServiceHandleName = serviceHandleName,
            GrpcEndpointUrl = grpcEndpointURL,
            InstanceId = instanceID,
            ServiceAssemblyName = serviceAssemblyName,
        };
        var wfoGrpcClient =
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
        var request = new QueryServiceInfoRequest
        {
            FromServiceCode = fromServiceCode,
            ToServiceCode = toServiceCode,
            InstanceId = instanceID,
        };
        var wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient
            .QueryServiceInfoAsync(request, _defaultHeader)
            .CallAsync<ServiceInfo>();
        return result;
    }

    public async Task<ServiceInfo> GetServiceInstanceByServiceHandleNameAsync(
        string serviceHandleName
    )
    {
        var request = new GetServiceInstanceByServiceHandleNameRequest
        {
            ServiceHandleName = serviceHandleName,
        };
        var wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient
            .GetServiceInstanceByServiceHandleNameAsync(request, _defaultHeader)
            .CallAsync<ServiceInfo>();
        return result;
    }

    public async Task SayHelloAsync(string name)
    {
        var streamClient = _grpcClientFactory.GetServerStreamAsync<
            WFOGrpcService.WFOGrpcServiceClient,
            HelloRequest,
            HelloReply
        >(
            (client, request, option) => client.SayHello(request, option),
            new HelloRequest { Name = name }
        );

        using var streamingCall = await streamClient;

        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine("Greeting: " + reply.Message);
        }
    }

    /// <summary>
    /// Ping Server
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<string> PingAsync(string name)
    {
        var request = new HelloRequest { Name = name };
        var wfoGrpcClient =
            await _grpcClientFactory.GetClientAsync<WFOGrpcService.WFOGrpcServiceClient>();
        var result = await wfoGrpcClient.PingAsync(request, _defaultHeader).CallAsync<string>();
        return result;
    }
}
