using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

public interface IVNPayTransactionStatusMapRepository : IRepository<VNPayTransactionStatusMap>
{
    public Task<string?> GetByStatusCodeAsync(string statusCode, CancellationToken cancellationToken = default);
}
