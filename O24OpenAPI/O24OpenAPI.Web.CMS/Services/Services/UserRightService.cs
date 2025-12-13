using LinqToDB;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class UserRightService(IRepository<UserRight> repository) : IUserRightService
{
    private readonly IRepository<UserRight> _repository = repository;

    public async Task UpdateAsync(UserRight userRight)
    {
        await _repository.Update(userRight);
    }

    public async Task AddAsync(UserRight userRight)
    {
        await _repository.InsertAsync(userRight);
    }

    public async Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId)
    {
        return await _repository
            .Table.Where(s => s.RoleId == roleId && s.CommandId == commandId)
            .FirstOrDefaultAsync();
    }
}
