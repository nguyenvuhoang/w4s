using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ZaloZNSTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ZaloZNSTemplate>(dataProvider, staticCacheManager), IZaloZNSTemplateRepository
{
    public Task<ZaloZNSTemplate?> FindByTemplateIdAsync(string templateId)
    {
        throw new NotImplementedException();
    }
}
