using LinKit.Core.Abstractions;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.CMS.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class FormRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Form>(dataProvider, staticCacheManager), IFormRepository
{ }
