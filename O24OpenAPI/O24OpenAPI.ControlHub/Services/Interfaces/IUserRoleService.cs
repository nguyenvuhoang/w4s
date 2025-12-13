using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserRoleService
{
    /// <summary>
    /// Add a user role entity asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<UserRole> AddAsync(UserRole entity);
    /// <summary>
    /// Get a user role entity by its ID asynchronously.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<UserRole> GetByRoleIdAsync(int roleId);
    /// <summary>
    /// Get the next available role ID asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<int> GetNextRoleIdAsync();
    /// <summary>
    /// Get all user role entities.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<List<UserRole>> GetAll();
    /// <summary>
    /// Get a user role entity by its Role Type asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<List<int>> GetByRoleTypeAsync(string roletype);

}
