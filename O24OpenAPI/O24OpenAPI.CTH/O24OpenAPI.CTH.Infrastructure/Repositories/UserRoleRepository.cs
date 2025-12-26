using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRoleRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserRole>(dataProvider, staticCacheManager), IUserRoleRepository
{
    public Task<bool> DeleteBulkAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UserRole> AddAsync(UserRole entity)
    {
        return await InsertAsync(entity);
    }

    public async Task<List<int>> GetByRoleTypeAsync(string roletype)
    {
        return await Table
            .Where(c => c.RoleType == roletype)
            .Select(x => x.RoleId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<int> GetNextRoleIdAsync()
    {
        var maxId = await Table.Select(x => (int?)x.RoleId).MaxAsync();

        return (maxId ?? 0) + 1;
    }

    public Task UpdateAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }
}
