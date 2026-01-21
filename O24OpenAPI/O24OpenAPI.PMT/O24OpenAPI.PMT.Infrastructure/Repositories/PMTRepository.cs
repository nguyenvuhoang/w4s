using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.PMT.Domain.AggregatesModel.PMTAggregate;

namespace O24OpenAPI.PMT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class PMTRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<PMTDomain>(dataProvider, staticCacheManager), IPMTRepository
{
    public void Add(PMTDomain entity)
    {
        // TODO: Implement
    }
}
