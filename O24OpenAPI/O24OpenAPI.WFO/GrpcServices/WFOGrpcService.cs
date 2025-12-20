using Grpc.Core;
using O24OpenAPI.Client.Lib;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Grpc;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.WFO;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Infrastructure;
using O24OpenAPI.WFO.Services.Interfaces;
using static O24OpenAPI.Grpc.WFO.WFOGrpcService;

namespace O24OpenAPI.WFO.GrpcServices;

public class WFOGrpcService : WFOGrpcServiceBase
{
    private readonly IServiceInstanceService _serviceInstanceService =
        EngineContext.Current.Resolve<IServiceInstanceService>();

    public override async Task<GrpcResponse> ExecuteWorkflow(
        ExecuteWorkflowRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var wfInputJson = request.WorkflowInputJson;
                if (wfInputJson.NullOrEmpty())
                {
                    throw new Exception("ExecuteWorkflowRequest is null or empty.");
                }
                var workflowExecutionGrpc = EngineContext.Current.Resolve<IWorkflowExecutionGrpc>();
                var wfResponse = await workflowExecutionGrpc.ExecuteWorkflowByGrpc(wfInputJson);
                return wfResponse;
            }
        );
    }

    public override async Task<GrpcResponse> RegisterServiceGrpcEndpoint(
        RegisterServiceGrpcEndpointRequest registerServiceGrpcEndpointRequest,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                string serviceCode = registerServiceGrpcEndpointRequest.ServiceCode;
                string serviceHandleName = registerServiceGrpcEndpointRequest.ServiceHandleName;
                string grpcEndpointURL = registerServiceGrpcEndpointRequest.GrpcEndpointUrl;
                string instanceID = registerServiceGrpcEndpointRequest.InstanceId;
                string assemblyName = registerServiceGrpcEndpointRequest.ServiceAssemblyName;
                long grpcTimeout =
                    EngineContext.Current.Resolve<WFOSetting>()?.GrpcTimeoutInSeconds ?? 60;
                var instance = new ServiceInstance()
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
                await _serviceInstanceService.AddAsync(instance);
                return "ok";
            }
        );
    }

    public override async Task<GrpcResponse> QueryServiceInfo(
        QueryServiceInfoRequest registerServiceGrpcEndpointRequest,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var pToServiceCode = registerServiceGrpcEndpointRequest.ToServiceCode;
                var pInstanceID = registerServiceGrpcEndpointRequest.InstanceId;
                if (pToServiceCode.NullOrEmpty())
                {
                    throw new ArgumentNullException(
                        nameof(pToServiceCode),
                        "Service code cannot be null or empty."
                    );
                }
                var result = await _serviceInstanceService.QueryServiceInfo(
                    pToServiceCode,
                    pInstanceID
                );
                return result;
            }
        );
    }

    public override async Task<GrpcResponse> GetServiceInstanceByServiceHandleName(
        GetServiceInstanceByServiceHandleNameRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var serviceHandleName = request.ServiceHandleName;
                if (serviceHandleName.NullOrEmpty())
                {
                    throw new ArgumentNullException(
                        nameof(serviceHandleName),
                        "Service handle name cannot be null or empty."
                    );
                }
                var result = await _serviceInstanceService.GetServiceInstanceByServiceHandleName(
                    serviceHandleName
                );
                return result;
            }
        );
    }

    public override async Task SayHello(
        HelloRequest request,
        IServerStreamWriter<HelloReply> responseStream,
        ServerCallContext context
    )
    {
        for (var i = 0; i < 5; i++)
        {
            var reply = new HelloReply { Message = "Hello " + request.Name + " " + i };
            await responseStream.WriteAsync(reply);
            await Task.Delay(1000);
        }
    }

    public override async Task<GrpcResponse> Ping(HelloRequest request, ServerCallContext context)
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                return $"Pong {request.Name}";
                ;
            }
        );
    }
}
