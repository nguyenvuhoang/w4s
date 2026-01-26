using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ChannelOverrideRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChannelOverride>(dataProvider, staticCacheManager),
        IChannelOverrideRepository { }
