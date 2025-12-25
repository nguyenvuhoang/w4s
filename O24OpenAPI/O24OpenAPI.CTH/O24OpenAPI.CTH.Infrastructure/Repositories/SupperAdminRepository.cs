using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SupperAdminRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SupperAdmin>(dataProvider, staticCacheManager), ISupperAdminRepository
{
    public async Task<SupperAdmin?> GetByUserNameAsync(string userName)
    {
        return await Table.Where(s => s.LoginName == userName).FirstOrDefaultAsync();
    }

    public async Task<SupperAdmin> IsExit()
    {
        return await Table.FirstOrDefaultAsync();
    }

    public async Task<bool> IsSupperAdminAsync(string userName)
    {
        return await Table.AnyAsync(s => s.LoginName == userName);
    }
}
