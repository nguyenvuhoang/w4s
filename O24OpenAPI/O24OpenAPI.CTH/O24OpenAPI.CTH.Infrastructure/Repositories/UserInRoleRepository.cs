using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserInRoleRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserInRole>(eventPublisher, dataProvider, staticCacheManager),
        IUserInRoleRepository
{
    public async Task<List<UserInRole>> GetUserInRolesByRoleIdAsync(int roleId)
    {
        return await Table.Where(x => x.RoleId == roleId).ToListAsync();
    }

    public async Task<List<UserInRole>> GetListRoleByUserCodeAsync(string userCode)
    {
        return await Table.Where(s => s.UserCode == userCode).ToListAsync();
    }

    public async Task DeleteByUserCodeAsync(string userCode)
    {
        List<UserInRole> roles = await Table.Where(x => x.UserCode == userCode).ToListAsync();
        if (roles.Count > 0)
        {
            await BulkDelete(roles);
        }
    }
}
