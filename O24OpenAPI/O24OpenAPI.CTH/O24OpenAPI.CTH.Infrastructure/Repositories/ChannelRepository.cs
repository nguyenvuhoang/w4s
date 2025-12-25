using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Channel>(dataProvider, staticCacheManager), IChannelRepository
{
    public async Task<List<Channel>> GetAllAsync(CancellationToken ct = default)
    {
        return await Table.OrderBy(c => c.SortOrder).ThenBy(c => c.ChannelId).ToListAsync(ct);
    }

    public async Task<Channel?> GetByChannelIdAsync(
        string channelId,
        CancellationToken ct = default
    )
    {
        return await Table.FirstOrDefaultAsync(c => c.ChannelId == channelId, ct);
    }
}
