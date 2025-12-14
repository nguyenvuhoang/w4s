using O24OpenAPI.AI.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;

namespace O24OpenAPI.AI.Infrastructure.Repositories;

public class SampleRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<Sample>(eventPublisher, dataProvider, staticCacheManager),
        ISampleRepository { }
