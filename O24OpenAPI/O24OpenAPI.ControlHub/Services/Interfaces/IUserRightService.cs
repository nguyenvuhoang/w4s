using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The user right service interface
/// </summary>
public interface IUserRightService
{
    /// <summary>
    /// Adds the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <returns>A task containing the user right</returns>
    Task<UserRightChannel> AddAsync(UserRightChannel entity);

    /// <summary>
    /// Gets the set channel in role using the specified role id
    /// </summary>
    /// <param name="roleId">The role id</param>
    /// <returns>A task containing a hash set of string</returns>
    Task<HashSet<string>> GetSetChannelInRoleAsync(int roleId);

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    Task UpdateAsync(UserRightChannel entity);
    Task<HashSet<string>> GetSetChannelInRoleAsync(int[] roleId);

    /// <summary>
    /// GetByRoleIdAndCommandIdAsync
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId);

    /// <summary>
    /// Update Async
    /// </summary>
    /// <param name="userRight"></param>
    /// <returns></returns>
    Task UpdateAsync(UserRight userRight);
    /// <summary>
    /// AddUserRightAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<UserRight> AddUserRightAsync(UserRight entity);
    /// <summary>
    /// Get list role id by channel async
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    Task<List<int>> GetListRoleIdByChannelAsync(string channelId);
}
