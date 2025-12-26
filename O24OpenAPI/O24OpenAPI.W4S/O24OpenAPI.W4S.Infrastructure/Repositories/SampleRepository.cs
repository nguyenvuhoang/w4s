using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

public class SampleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Sample>(dataProvider, staticCacheManager), ISampleRepository { }
