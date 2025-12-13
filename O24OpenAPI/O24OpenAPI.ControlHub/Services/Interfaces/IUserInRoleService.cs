using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserInRoleService
{
    /// <summary>
    /// Add a user in role
    /// </summary>
    /// <param name="userInRole"></param>
    /// <returns></returns>
    Task<UserInRole> AddAsync(UserInRole userInRole);
    /// <summary>
    /// Bulk insert user in roles
    /// </summary>
    /// <param name="userInRole"></param>
    /// <returns></returns>
    Task<List<UserInRole>> BulkInsert(List<UserInRole> userInRole);
    /// <summary>
    /// Get user in role by user id
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<List<UserInRole>> GetUserInRolesByRoleIdAsync(int roleId);
    /// <summary>
    /// Delete a user in role by user id and role id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<bool> DeleteBulkAsync(List<UserInRole> listUserInRole);
    /// <summary>
    /// Get List Role By User Code Async
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<List<UserInRole>> GetListRoleByUserCodeAsync(string userCode);
    /// <summary>
    /// Delete User In Role By User Code Async
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task DeleteByUserCodeAsync(string userCode);
}
