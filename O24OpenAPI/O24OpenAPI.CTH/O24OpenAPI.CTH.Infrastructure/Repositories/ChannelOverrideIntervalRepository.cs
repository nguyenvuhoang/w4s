using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelOverrideIntervalRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelOverrideInterval>(dataProvider, staticCacheManager),
        IChannelOverrideIntervalRepository
{
    public async Task<List<ChannelOverrideInterval>> GetByOverrideIdAsync(
        int overrideId,
        CancellationToken ct = default
    )
    {
        return await Table
            .Where(iv => iv.ChannelOverrideIdRef == overrideId)
            .OrderBy(iv => iv.SortOrder)
            .ToListAsync(ct);
    }
}
