using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelUserOverrideRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelUserOverride>(eventPublisher, dataProvider, staticCacheManager),
        IChannelUserOverrideRepository
{

public async Task<ChannelUserOverride?> GetByChannelAndUserAsync(string channelId, string userCode, CancellationToken ct = default)
{
    return await Table.FirstOrDefaultAsync(o => o.ChannelIdRef == channelId && o.UserCode == userCode, ct);
}

public async Task<List<ChannelUserOverride>> GetByChannelAsync(string channelId, CancellationToken ct = default)
{
    return await Table.Where(o => o.ChannelIdRef == channelId).ToListAsync(ct);
}
}
