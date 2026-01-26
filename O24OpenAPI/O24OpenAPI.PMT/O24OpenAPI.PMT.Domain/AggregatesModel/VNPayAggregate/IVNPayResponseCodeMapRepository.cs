using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

public interface IVNPayResponseCodeMapRepository : IRepository<VNPayResponseCodeMap>
{
    public Task<string?> GetByResponseCodeAsync(string responseCode, CancellationToken cancellationToken = default);
}
