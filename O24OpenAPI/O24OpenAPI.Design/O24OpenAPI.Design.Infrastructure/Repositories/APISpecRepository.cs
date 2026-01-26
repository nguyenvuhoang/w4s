using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Design.Domain.AggregatesModel.DesignAggregate;

namespace O24OpenAPI.Design.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class APISpecRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<APISpec>(dataProvider, staticCacheManager), IAPISpecRepository
{
    public async Task<IReadOnlyList<APISpec>> GetByServiceIdAsync(int apiServiceId) =>
        await Table.Where(x => x.ApiServiceId == apiServiceId).ToListAsync();

    public async Task<APISpec?> GetByServiceIdAndVersionAsync(int apiServiceId, string version, string specFormat) =>
        await Table.FirstOrDefaultAsync(x =>
            x.ApiServiceId == apiServiceId &&
            x.Version == version &&
            x.SpecFormat == specFormat
        );
}
