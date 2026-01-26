using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

namespace O24OpenAPI.PMT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class VNPayTransactionResponseMapRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<VNPayResponseCodeMap>(dataProvider, staticCacheManager), IVNPayResponseCodeMapRepository
{
    public async Task<string?> GetByResponseCodeAsync(string responseCode, CancellationToken cancellationToken = default)
    {
        return await Table.Where(x => x.ResponseCode == responseCode)
             .Select(x => x.Description)
             .FirstOrDefaultAsync(cancellationToken);
    }
}