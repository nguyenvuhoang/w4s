using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Design.Domain.AggregatesModel.MiscAggregate;

namespace O24OpenAPI.Design.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SampleDomainRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SampleDomain>(dataProvider, staticCacheManager), ISampleDomainRepository { }
