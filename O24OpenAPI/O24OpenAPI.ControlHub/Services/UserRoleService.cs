using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;

namespace O24OpenAPI.ControlHub.Services;

public class UserRoleService(
    IRepository<UserRole> userRoleRepository
) : IUserRoleService
{
    private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;

    public async Task<UserRole> AddAsync(UserRole entity)
    {
        return await _userRoleRepository.InsertAsync(entity);
    }


    /// <summary>
    /// Get User Role by RoleId
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<UserRole> GetByRoleIdAsync(int roleId)
    {
        return await _userRoleRepository.Table.Where(s => s.RoleId == roleId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get the next available role ID asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetNextRoleIdAsync()
    {
        var maxId = await _userRoleRepository.Table
            .Select(x => (int?)x.RoleId)
            .MaxAsync();

        return (maxId ?? 0) + 1;
    }
    /// <summary>
    /// Get all user role entities.
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserRole>> GetAll()
    {
        return await _userRoleRepository.Table.ToListAsync();
    }

    public async Task<List<int>> GetByRoleTypeAsync(string roletype)
    {
        return await _userRoleRepository.Table
            .Where(c => c.RoleType == roletype)
            .Select(x => x.RoleId)
            .Distinct()
            .ToListAsync();
    }
}
