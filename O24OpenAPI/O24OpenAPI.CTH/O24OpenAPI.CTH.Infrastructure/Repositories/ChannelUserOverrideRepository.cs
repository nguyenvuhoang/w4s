using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelUserOverrideRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelUserOverride>(dataProvider, staticCacheManager),
        IChannelUserOverrideRepository
{
    public async Task<ChannelUserOverride?> GetByChannelAndUserAsync(
        string channelId,
        string userCode,
        CancellationToken ct = default
    )
    {
        return await Table.FirstOrDefaultAsync(
            o => o.ChannelIdRef == channelId && o.UserCode == userCode,
            ct
        );
    }

    public async Task<List<ChannelUserOverride>> GetByChannelAsync(
        string channelId,
        CancellationToken ct = default
    )
    {
        return await Table.Where(o => o.ChannelIdRef == channelId).ToListAsync(ct);
    }
}
