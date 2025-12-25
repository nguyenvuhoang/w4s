using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelUserOverrideIntervalRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelUserOverrideInterval>(dataProvider, staticCacheManager),
        IChannelUserOverrideIntervalRepository
{
    public async Task<List<ChannelUserOverrideInterval>> GetByOverrideIdsAsync(
        ICollection<int> overrideIds,
        CancellationToken ct = default
    )
    {
        if (overrideIds == null || overrideIds.Count == 0)
        {
            return [];
        }

        return await Table
            .Where(iv => overrideIds.Contains(iv.ChannelUserOverrideIdRef))
            .OrderBy(iv => iv.SortOrder)
            .ToListAsync(ct);
    }
}
