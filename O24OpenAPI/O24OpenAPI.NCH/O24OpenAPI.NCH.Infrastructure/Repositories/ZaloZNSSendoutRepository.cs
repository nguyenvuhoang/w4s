using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories
{
    [RegisterService(Lifetime.Scoped)]
    public class ZaloZNSSendoutRepository(
        IO24OpenAPIDataProvider dataProvider,
        IStaticCacheManager staticCacheManager
    ) : EntityRepository<ZaloZNSSendout>(dataProvider, staticCacheManager), IZaloZNSSendoutRepository
    {
        public async Task<bool> ExistsByRefIdAsync(string refId, CancellationToken ct = default)
        {
            var exists = await Table.Where(e => e.RefId == refId).FirstOrDefaultAsync();
            return exists != null;
        }
    }

}

