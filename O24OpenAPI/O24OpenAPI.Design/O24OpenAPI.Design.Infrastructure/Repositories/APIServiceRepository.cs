using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Design.Domain.AggregatesModel.DesignAggregate;

namespace O24OpenAPI.Design.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class APIServiceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<APIService>(dataProvider, staticCacheManager), IAPIServiceRepository
{
    public async Task<APIService?> GetByCodeAsync(string serviceCode) =>
        await Table.FirstOrDefaultAsync(x => x.ServiceCode == serviceCode);

    public async Task<IReadOnlyList<APIService>> GetAllActiveAsync() =>
        await Table.Where(x => x.IsActive).ToListAsync();
}
