using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Services.Interfaces;

public interface IServiceInstanceService
{
    Task AddAsync(ServiceInstance serviceInstance);
    Task<ServiceInstance> GetByServiceCodeAsync(string serviceCode);
    Task<ServiceInfo> QueryServiceInfo(string pServiceCode, string pInstanceID);
    Task<ServiceInfo> GetServiceInstanceByServiceHandleName(string serviceHandleName);
}
