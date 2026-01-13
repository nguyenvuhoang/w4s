using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.EXT.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.EXT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SampleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SampleDomain>(dataProvider, staticCacheManager), ISampleRepository
{
    public void Add(SampleDomain sample)
    {
        //viết linq;
        //var sampale = await Table.Where(s=>s.Id == sample.Id).FirstOrDefaultAsync();
    }
}
