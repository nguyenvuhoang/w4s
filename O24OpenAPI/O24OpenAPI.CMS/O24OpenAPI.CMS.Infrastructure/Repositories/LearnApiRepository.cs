using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;

namespace O24OpenAPI.CMS.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class LearnApiRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<LearnApi>(eventPublisher, dataProvider, staticCacheManager),
        ILearnApiRepository
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public virtual async Task<LearnApi> GetByChannelAndLearnApiIdAsync(
        string channel,
        string learnApiId
    )
    {
        CacheKey cacheKey = CachingKey.EntityKey<LearnApi>(channel, learnApiId);
        var result =
            await _staticCacheManager.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    var query = Table.Where(s =>
                        s.Channel == channel && s.LearnApiId == learnApiId
                    );

                    var learnApi = await query.FirstOrDefaultAsync();

                    if (learnApi == null)
                    {
                        return null;
                    }

                    return learnApi;
                }
            ) ?? throw new InvalidOperationException($"LearnApi {channel}:{learnApiId} not found");
        return result;
    }
}
