using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;

public interface IVNPayTransactionStatusMapRepository : IRepository<VNPayTransactionStatusMap>
{
    public Task<string?> GetByStatusCodeAsync(string statusCode, CancellationToken cancellationToken = default);
}
