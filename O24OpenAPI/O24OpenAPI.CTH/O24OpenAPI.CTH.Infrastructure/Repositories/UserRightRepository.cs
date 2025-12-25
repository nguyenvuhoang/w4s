using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRightRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserRight>(dataProvider, staticCacheManager), IUserRightRepository
{
    public async Task<List<string>> GetExistingCommandIdsAsync(
        int roleId,
        ICollection<string> commandIds
    )
    {
        if (commandIds == null || commandIds.Count == 0)
        {
            return [];
        }

        return await Table
            .Where(r => r.RoleId == roleId && commandIds.Contains(r.CommandId))
            .Select(r => r.CommandId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId)
    {
        return await Table
            .Where(s => s.RoleId == roleId && s.CommandId == commandId)
            .FirstOrDefaultAsync();
    }

    public async Task<UserRight> AddUserRightAsync(UserRight entity)
    {
        return await InsertAsync(entity);
    }

    public async Task UpdateAsync(UserRight userRight)
    {
        await Update(userRight);
    }
}
