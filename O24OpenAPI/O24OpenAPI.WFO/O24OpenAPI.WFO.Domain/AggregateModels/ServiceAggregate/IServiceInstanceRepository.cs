using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.WFO.Domain.AggregateModels.ServiceAggregate;

public interface IServiceInstanceRepository : IRepository<ServiceInstance>
{
    Task AddAsync(ServiceInstance serviceInstance);
    Task<ServiceInfo> QueryServiceInfo(string pServiceCode, string pInstanceID);
    Task<ServiceInfo> GetServiceInstanceByServiceHandleName(string serviceHandleName);
}
