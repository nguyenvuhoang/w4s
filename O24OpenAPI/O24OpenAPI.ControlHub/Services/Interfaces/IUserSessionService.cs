using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The user session service interface
/// </summary>
public partial interface IUserSessionService
{
    /// <summary>
    /// Inserts the user session
    /// </summary>
    /// <param name="userSession">The user session</param>
    Task Insert(UserSession userSession);

    /// <summary>
    /// Gets the by token using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="activeOnly">The active only</param>
    /// <returns>A task containing the user session</returns>
    Task<UserSession> GetByToken(string token, bool activeOnly = true);

    /// <summary>
    /// Deletes the token using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    Task DeleteToken(string token);

    /// <summary>
    /// Gets the active by login name using the specified login name
    /// </summary>
    /// <param name="loginName">The login name</param>
    /// <returns>A task containing the user session</returns>
    Task<UserSession> GetActiveByLoginName(string loginName);

    /// <summary>
    /// Checks the valid single session using the specified login name
    /// </summary>
    /// <param name="loginName">The login name</param>
    /// <param name="ipAddress">The ip address</param>
    /// <param name="isUpdateOldSession">The is update old session</param>
    /// <returns>A task containing the validate session model</returns>
    Task<ValidateSessionModel> CheckValidSingleSession(
        string loginName,
        string language = "en"
    );

    Task RevokeByLoginName(string loginName);

    Task<UserSession> GetByRefreshToken(string token);

    Task Revoke(string token);
}
