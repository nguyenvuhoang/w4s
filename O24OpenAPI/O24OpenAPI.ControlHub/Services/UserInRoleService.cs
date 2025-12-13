using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;

namespace O24OpenAPI.ControlHub.Services;

public class UserInRoleService(IRepository<UserInRole> userInRoleRepository)
    : IUserInRoleService
{
    private readonly IRepository<UserInRole> _userInRole = userInRoleRepository;

    public async Task<UserInRole> AddAsync(UserInRole userInRole)
    {
        return await _userInRole.InsertAsync(userInRole);
    }

    public async Task<List<UserInRole>> BulkInsert(List<UserInRole> userInRole)
    {
        await _userInRole.BulkInsert(userInRole);
        return userInRole;
    }

    public async Task<List<UserInRole>> GetUserInRolesByRoleIdAsync(int roleId)
    {
        return await _userInRole.Table.Where(x => x.RoleId == roleId).ToListAsync();
    }

    public async Task<bool> DeleteBulkAsync(List<UserInRole> listUserInRole)
    {
        await _userInRole.BulkDelete(listUserInRole);
        return true;
    }

    public async Task<List<UserInRole>> GetListRoleByUserCodeAsync(string userCode)
    {
        var getRoleList = await _userInRole
            .Table.Where(s => s.UserCode.Equals(userCode))
            .ToListAsync();
        return getRoleList ?? [];
    }

    public async Task DeleteByUserCodeAsync(string userCode)
    {
        var roles = await _userInRole.Table
            .Where(x => x.UserCode == userCode)
            .ToListAsync();

        foreach (var role in roles)
        {
            await _userInRole.Delete(role);
        }
    }
}
