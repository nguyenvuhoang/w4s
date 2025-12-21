using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelOverrideIntervalRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelOverrideInterval>(eventPublisher, dataProvider, staticCacheManager),
        IChannelOverrideIntervalRepository
{

public async Task<List<ChannelOverrideInterval>> GetByOverrideIdAsync(int overrideId, CancellationToken ct = default)
{
    return await Table
        .Where(iv => iv.ChannelOverrideIdRef == overrideId)
        .OrderBy(iv => iv.SortOrder)
        .ToListAsync(ct);
}
}
