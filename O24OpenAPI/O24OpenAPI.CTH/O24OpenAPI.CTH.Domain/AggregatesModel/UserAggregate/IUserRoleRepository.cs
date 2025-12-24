using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<bool> DeleteBulkAsync();
    Task UpdateAsync(UserAccount userAccount);
    Task<List<int>> GetByRoleTypeAsync(string roletype);
    Task<int> GetNextRoleIdAsync();
    Task<UserRole> AddAsync(UserRole entity);
}
