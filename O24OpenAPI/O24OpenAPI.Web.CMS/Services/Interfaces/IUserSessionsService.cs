using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IUserSessionsService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="userSession"></param>
    /// <returns></returns>
    Task Insert(UserSessions userSession);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="mac"></param>
    /// <returns></returns>
    Task UpdateSessionMac(string token, string mac);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="activeOnly"></param>
    /// <returns></returns>
    Task<UserSessions> GetByToken(string token, bool activeOnly = true);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    Task UpdateInfo(string token, string info);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="mac"></param>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    Task UpdateSessionMacAndApplicationCode(string token, string mac, string applicationCode);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    Task UpdateSessionAndApplicationCode(string token, string applicationCode);

    Task UpdateActtype(string token, string acttype, bool isLockoutOldSession = false);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    Task UpdateSessionAndInfo(string token, string info);

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task DeleteToken(string token);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="applicationCode"></param>
    /// <param name="currentToken"></param>
    /// <returns></returns>
    Task DeleteAllAppToken(string userId, string applicationCode, string currentToken);

    /// <summary>
    ///
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="exxpireTime"></param>
    /// <returns></returns>
    Task<UserSessions> GetListUserSessionByLoginNameAndExpireTime(
        string loginName,
        DateTimeOffset exxpireTime
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="mac"></param>
    /// <returns></returns>
    Task<List<UserSessions>> GetListUserSessionByLoginNameAndNotEqualMac(
        string loginName,
        string mac
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteAllById(string userId);
    Task<UserSessions> GetActiveByLoginName(string loginName);
    Task<ValidateSessionModel> CheckValidSingleSession(
        string loginName,
        bool isUpdateOldSession = false
    );
    Task<(bool isValid, UserSessions userSession)> CheckValidSession(string token);
    Task<UserSessions> GetByRefreshToken(string token);
    Task ClearSession(string loginName);
    Task<(UserSessions, string, string)> RefreshSession(UserSessions userSessions);
    Task UpdateSignatureKey(string token, string signatureKey);
}
