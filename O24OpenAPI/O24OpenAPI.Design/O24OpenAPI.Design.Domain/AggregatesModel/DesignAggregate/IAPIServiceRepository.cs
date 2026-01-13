using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Design.Domain.AggregatesModel.DesignAggregate;

public interface IAPIServiceRepository : IRepository<APIService>
{
    Task<APIService?> GetByCodeAsync(string serviceCode);

    Task<IReadOnlyList<APIService>> GetAllActiveAsync();
}
