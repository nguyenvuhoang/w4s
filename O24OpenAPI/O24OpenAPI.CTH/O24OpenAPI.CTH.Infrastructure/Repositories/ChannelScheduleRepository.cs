using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelScheduleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ChannelSchedule>(dataProvider, staticCacheManager), IChannelScheduleRepository
{
    public async Task<List<ChannelSchedule>> GetByChannelIdsAsync(
        ICollection<int> channelIds,
        CancellationToken ct = default
    )
    {
        if (channelIds == null || channelIds.Count == 0)
        {
            return [];
        }

        return await Table.Where(s => channelIds.Contains(s.ChannelIdRef)).ToListAsync(ct);
    }

    public async Task<ChannelSchedule?> GetByChannelAndDayOfWeekAsync(
        int channelIdRef,
        DayOfWeek dayOfWeek,
        CancellationToken ct = default
    )
    {
        return await Table.FirstOrDefaultAsync(
            s => s.ChannelIdRef == channelIdRef && s.DayOfWeek == dayOfWeek,
            ct
        );
    }
}
