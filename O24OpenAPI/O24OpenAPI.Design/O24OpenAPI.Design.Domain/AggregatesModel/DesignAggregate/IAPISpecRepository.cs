using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Design.Domain.AggregatesModel.DesignAggregate;

public interface IAPISpecRepository : IRepository<APISpec>
{
    Task<IReadOnlyList<APISpec>> GetByServiceIdAsync(int apiServiceId);

    Task<APISpec?> GetByServiceIdAndVersionAsync(int apiServiceId, string version, string specFormat);
}
