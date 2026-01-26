using Grpc.Core;
using O24OpenAPI.Client.Lib;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.WFO.API.Application.Features.WorkflowExecutions;
using O24OpenAPI.WFO.Domain.AggregateModels.ServiceAggregate;
using O24OpenAPI.WFO.Infrastructure.Configurations;
using static O24OpenAPI.Grpc.WFO.WFOGrpcService;

namespace O24OpenAPI.WFO.API.GrpcServices;

public class WFOGrpcService : WFOGrpcServiceBase
{
    private readonly IServiceInstanceRepository serviceInstanceRepository =
        EngineContext.Current.Resolve<IServiceInstanceRepository>();

    public override async Task<GrpcResponse> ExecuteWorkflow(
        ExecuteWorkflowRequest request,
        ServerCallContext context
    )
    {
        string wfInputJson = request.WorkflowInputJson;
        if (wfInputJson.NullOrEmpty())
        {
            throw new Exception("ExecuteWorkflowRequest is null or empty.");
        }
        IWorkflowExecutionHandler workflowExecutionHandler =
            EngineContext.Current.Resolve<IWorkflowExecutionHandler>();
        var wfResponse = workflowExecutionHandler.ExecuteWorkflowAsync(wfInputJson);
        return await wfResponse.GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> RegisterServiceGrpcEndpoint(
        RegisterServiceGrpcEndpointRequest registerServiceGrpcEndpointRequest,
        ServerCallContext context
    )
    {
        string serviceCode = registerServiceGrpcEndpointRequest.ServiceCode;
        string serviceHandleName = registerServiceGrpcEndpointRequest.ServiceHandleName;
        string grpcEndpointURL = registerServiceGrpcEndpointRequest.GrpcEndpointUrl;
        string instanceID = registerServiceGrpcEndpointRequest.InstanceId;
        string assemblyName = registerServiceGrpcEndpointRequest.ServiceAssemblyName;
        long grpcTimeout = EngineContext.Current.Resolve<WFOSetting>()?.GrpcTimeoutInSeconds ?? 60;
        ServiceInstance instance = new()
        {
            ServiceCode = serviceCode,
            ServiceHandleName = serviceHandleName,
            GrpcUrl = grpcEndpointURL,
            InstanceID = instanceID,
            EventQueueName = QueueUtils.GetEventQueueName(serviceCode: serviceCode),
            CommandQueueName = QueueUtils.GetCommandQueueName(serviceCode: serviceCode),
            AssemblyName = assemblyName,
            GrpcTimeout = grpcTimeout,
        };
        await serviceInstanceRepository.AddAsync(instance);
        return await Task.FromResult("ok").GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> QueryServiceInfo(
        QueryServiceInfoRequest registerServiceGrpcEndpointRequest,
        ServerCallContext context
    )
    {
        string pToServiceCode = registerServiceGrpcEndpointRequest.ToServiceCode;
        string pInstanceID = registerServiceGrpcEndpointRequest.InstanceId;
        if (pToServiceCode.NullOrEmpty())
        {
            throw new ArgumentNullException(
                nameof(pToServiceCode),
                "Service code cannot be null or empty."
            );
        }
        var result = serviceInstanceRepository.QueryServiceInfo(pToServiceCode, pInstanceID);
        return await result.GetGrpcResponseAsync();
    }

    public override async Task<GrpcResponse> GetServiceInstanceByServiceHandleName(
        GetServiceInstanceByServiceHandleNameRequest request,
        ServerCallContext context
    )
    {
        string serviceHandleName = request.ServiceHandleName;
        if (serviceHandleName.NullOrEmpty())
        {
            throw new ArgumentNullException(
                nameof(serviceHandleName),
                "Service handle name cannot be null or empty."
            );
        }
        var result = serviceInstanceRepository.GetServiceInstanceByServiceHandleName(
            serviceHandleName
        );
        return await result.GetGrpcResponseAsync();
    }

    public override async Task SayHello(
        HelloRequest request,
        IServerStreamWriter<HelloReply> responseStream,
        ServerCallContext context
    )
    {
        for (int i = 0; i < 5; i++)
        {
            HelloReply reply = new() { Message = "Hello " + request.Name + " " + i };
            await responseStream.WriteAsync(reply);
            await Task.Delay(1000);
        }
    }

    public override async Task<GrpcResponse> Ping(HelloRequest request, ServerCallContext context)
    {
        return await Task.FromResult($"Pong {request.Name}").GetGrpcResponseAsync();
    }
}
