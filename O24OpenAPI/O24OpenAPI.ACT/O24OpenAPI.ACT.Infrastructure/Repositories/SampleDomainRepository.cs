using LinKit.Core.Abstractions;
using O24OpenAPI.ACT.Domain.AggregatesModel.MiscAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SampleDomainRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SampleDomain>(dataProvider, staticCacheManager), ISampleDomainRepository { }
