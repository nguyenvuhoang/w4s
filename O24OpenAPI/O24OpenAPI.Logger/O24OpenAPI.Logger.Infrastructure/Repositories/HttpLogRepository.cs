using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Logger.Domain.AggregateModels.HttpLogAggregate;

namespace O24OpenAPI.Logger.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class HttpLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<HttpLog>(dataProvider, staticCacheManager), IHttpLogRepository
{ }
