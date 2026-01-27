using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories
{
    [RegisterService(Lifetime.Scoped)]
    public class ZaloOATokenRepository(
        IO24OpenAPIDataProvider dataProvider,
        IStaticCacheManager staticCacheManager
    ) : EntityRepository<ZaloOAToken>(dataProvider, staticCacheManager), IZaloOATokenRepository
    {
        public async Task DeactivateActiveAsync(string oaId, CancellationToken ct)
        {
            var activeToken = await Table
                 .Where(t => t.OaId == oaId && t.IsActive)
                 .FirstOrDefaultAsync(token: ct);

            if (activeToken == null)
                return;
            activeToken.IsActive = false;
            await Update(activeToken);
        }

        public async Task<ZaloOAToken?> GetActiveByOaIdAsync(string oaId, CancellationToken ct)
        {
            return await Table.Where(t => t.OaId == oaId && t.IsActive)
                .FirstOrDefaultAsync(ct);
        }

        public async Task UpdateLastUsedAsync(string oaId, CancellationToken ct)
        {
            var activeToken = await Table
                  .Where(t => t.OaId == oaId && t.IsActive)
                  .FirstOrDefaultAsync(token: ct);

            if (activeToken == null)
                return;
            activeToken.LastUsedAtUtc = DateTime.UtcNow;
            await Update(activeToken);
        }
    }

}

