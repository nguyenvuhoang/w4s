using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;

public interface IVNPayResponseCodeMapRepository : IRepository<VNPayResponseCodeMap>
{
    public Task<string?> GetByResponseCodeAsync(string responseCode, CancellationToken cancellationToken = default);
}
