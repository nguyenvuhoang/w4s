using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelScheduleIntervalRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelScheduleInterval>(eventPublisher, dataProvider, staticCacheManager),
        IChannelScheduleIntervalRepository
{

public async Task<List<ChannelScheduleInterval>> GetByScheduleIdsAsync(ICollection<int> scheduleIds, CancellationToken ct = default)
{
    if (scheduleIds == null || scheduleIds.Count == 0)
    {
        return [];
    }

    return await Table
        .Where(iv => scheduleIds.Contains(iv.ChannelScheduleIdRef))
        .OrderBy(iv => iv.SortOrder)
        .ToListAsync(ct);
}
}
