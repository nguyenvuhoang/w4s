using O24OpenAPI.Contracts.Configuration.Client;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;

public interface IWFOGrpcClientService
{
    Task<string> ExecuteWorkflowAsync(string workflowInputJson);
    Task<string> RegisterServiceGrpcEndpointAsync(
        string serviceCode,
        string serviceHandleName,
        string grpcEndpointURL,
        string instanceID,
        string serviceAssemblyName
    );
    Task<ServiceInfo> QueryServiceInfoAsync(
        string fromServiceCode,
        string toServiceCode,
        string instanceID
    );
    Task<ServiceInfo> GetServiceInstanceByServiceHandleNameAsync(string serviceHandleName);

    Task SayHelloAsync(string name);
    Task<string> PingAsync(string name);
}
