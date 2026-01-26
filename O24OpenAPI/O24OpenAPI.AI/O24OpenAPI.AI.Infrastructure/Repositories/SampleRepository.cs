using LinKit.Core.Abstractions;
using O24OpenAPI.AI.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.AI.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SampleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Sample>(dataProvider, staticCacheManager), ISampleRepository
{
    public void Add(Sample sample)
    {
        //viết linq;
        //var sampale = await Table.Where(s=>s.Id == sample.Id).FirstOrDefaultAsync();
    }
}
