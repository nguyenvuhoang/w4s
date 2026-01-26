using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.DWH.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.DWH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SampleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SampleDomain>(dataProvider, staticCacheManager), ISampleRepository
{
    public void Add(SampleDomain sample)
    {
        //vi?t linq;
        //var sampale = await Table.Where(s=>s.Id == sample.Id).FirstOrDefaultAsync();
    }
}
