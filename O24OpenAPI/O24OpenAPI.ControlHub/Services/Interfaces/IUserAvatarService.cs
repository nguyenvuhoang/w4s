using O24OpenAPI.ControlHub.Domain;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The user avatar service interface
/// </summary>
public interface IUserAvatarService
{

    Task<UserAvatar> AddAsync(UserAvatar user);
    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<UserAvatar> GetByUserCodeAsync(string userCode);
    /// <summary>
    /// Update UserAccount
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(UserAvatar entity);
}
