using LinKit.Core.Abstractions;
using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;

namespace O24OpenAPI.CMS.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class AppRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<App>(eventPublisher, dataProvider, staticCacheManager), IAppRepository
{ }
