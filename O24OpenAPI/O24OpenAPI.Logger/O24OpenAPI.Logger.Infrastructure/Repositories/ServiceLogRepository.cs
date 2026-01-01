using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ServiceLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ServiceLog>(dataProvider, staticCacheManager), IServiceLogRepository
{ }
