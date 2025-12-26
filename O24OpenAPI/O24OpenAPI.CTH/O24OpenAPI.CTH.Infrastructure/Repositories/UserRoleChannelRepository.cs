using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRoleChannelRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserRoleChannel>(dataProvider, staticCacheManager), IUserRoleChannelRepository
{
    public async Task<List<UserRoleChannel>> GetByRoleIdAsync(int roleId)
    {
        return await Table.Where(x => x.RoleId == roleId).ToListAsync();
    }
}
