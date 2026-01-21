using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.CMS.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class VNPayTransactionStatusMapRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<VNPayTransactionStatusMap>(dataProvider, staticCacheManager), IVNPayTransactionStatusMapRepository
{
    public async Task<string?> GetByStatusCodeAsync(string statusCode, CancellationToken cancellationToken = default)
    {
        return await Table.Where(x => x.StatusCode == statusCode)
             .Select(x => x.StatusMessage)
             .FirstOrDefaultAsync(cancellationToken);
    }
}