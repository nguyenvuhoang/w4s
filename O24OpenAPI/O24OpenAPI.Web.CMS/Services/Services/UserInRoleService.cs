using LinqToDB;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// Role of user service constructor
/// </summary>
/// <param name="roleOfUserRepository"></param>
public class UserInRoleService(IRepository<UserInRole> roleOfUserRepository)
    : IUserInRoleService
{
    private readonly IRepository<UserInRole> _roleOfUserRepository = roleOfUserRepository;

    public async Task<UserInRole> Create(UserInRole userInRole)
    {
        return await _roleOfUserRepository.InsertAsync(userInRole);
    }

    public async Task Delete(UserInRole userInRole)
    {
        await _roleOfUserRepository.Delete(userInRole);
    }

    public Task<UserInRole> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserInRole>> GetListUserByRoleId(int roleId)
    {
        return await _roleOfUserRepository.Table.Where(s => s.RoleId == roleId).ToListAsync();
    }

    public async Task<UserInRole> GetByRoleIdAndUserCode(int roleId, string userCode)
    {
        return await _roleOfUserRepository
            .Table.Where(s => s.RoleId == roleId && s.UserCode == userCode)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<List<UserInRole>> GetListRoleByUserCode(string userCode)
    {
        var getRoleList = await _roleOfUserRepository
            .Table.Where(s => s.UserCode.Equals(userCode))
            .ToListAsync();
        return getRoleList ?? new List<UserInRole>();
    }

    public Task<bool> IsRoleHavingUser(int roleId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserHaveRole(int userAccountId)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<UserInRoleSearchResponseModel>> Search(SimpleSearchModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<UserInRoleSearchResponseModel>> Search(UserInRoleSearchModel model)
    {
        throw new NotImplementedException();
    }

    public Task Update(UserInRole UserInRole, string referenceId = "")
    {
        throw new NotImplementedException();
    }
}
