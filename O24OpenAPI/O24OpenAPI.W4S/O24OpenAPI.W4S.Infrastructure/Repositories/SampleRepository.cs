using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

public class SampleRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<Sample>(eventPublisher, dataProvider, staticCacheManager),
        ISampleRepository { }
