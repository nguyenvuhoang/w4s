using Grpc.Core;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.Factory;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;

public class WFOGrpcClientService : BaseGrpcClientService, IWFOGrpcClientService
{
    public WFOGrpcClientService()
    {
        ServerId = "WFO";
    }

    private readonly IGrpcClient<WFOGrpcService.WFOGrpcServiceClient> _wfoGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<WFOGrpcService.WFOGrpcServiceClient>>();

    public async Task<string> ExecuteWorkflowAsync(string workflowInputJson)
    {
        var executeWorkflowRequest = new ExecuteWorkflowRequest
        {
            WorkflowInputJson = workflowInputJson,
        };
        var result = await InvokeAsync<string>(
            async (header) =>
            {
                return await _wfoGrpcClient.Client.ExecuteWorkflowAsync(
                    executeWorkflowRequest,
                    header
                );
            }
        );
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
        return await InvokeAsync<string>(
            async (header) =>
            {
                return await _wfoGrpcClient.Client.RegisterServiceGrpcEndpointAsync(
                    request,
                    header
                );
            }
        );
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
        return await InvokeAsync<ServiceInfo>(
            async (header) =>
            {
                return await _wfoGrpcClient.Client.QueryServiceInfoAsync(request, header);
            }
        );
        ;
    }

    public async Task<ServiceInfo> GetServiceInstanceByServiceHandleNameAsync(
        string serviceHandleName
    )
    {
        var request = new GetServiceInstanceByServiceHandleNameRequest
        {
            ServiceHandleName = serviceHandleName,
        };
        return await InvokeAsync<ServiceInfo>(
            async (header) =>
            {
                return await _wfoGrpcClient.Client.GetServiceInstanceByServiceHandleNameAsync(
                    request,
                    header
                );
            }
        );
    }

    public async Task SayHelloAsync(string name)
    {
        var grpcFactory = EngineContext.Current.Resolve<IGrpcClientFactory>();

        var streamClient = grpcFactory.GetServerStreamAsync<
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
        var result = await InvokeAsync<string>(
            async (header) =>
            {
                return await _wfoGrpcClient.Client.PingAsync(request, header);
            }
        );
        return result;
    }
}
