namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IUserRightService
{
    Task UpdateAsync(UserRight userRight);
    Task AddAsync(UserRight userRight);
    Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId);
}
