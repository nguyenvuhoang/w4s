using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain.Users;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class UserSessionsService(
    IRepository<UserSessions> userSessionsRepository,
    IStaticCacheManager staticCacheManager,
    IJwtTokenService jwtTokenService,
    JWebUIObjectContextModel context
) : IUserSessionsService
{
    private readonly IRepository<UserSessions> _userSessionsRepository = userSessionsRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly JWebUIObjectContextModel _context = context;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    /// <summary>
    ///
    /// </summary>
    /// <param name="userSession"></param>
    /// <returns></returns>
    public virtual async Task Insert(UserSessions userSession)
    {
        ArgumentNullException.ThrowIfNull(userSession);
        await _userSessionsRepository.Insert(userSession);
        var cacheKey = CachingKey.SessionKey(userSession.Token);
        await _staticCacheManager.Set(cacheKey, userSession);
    }

    public async Task UpdateAsync(UserSessions userSession, string token)
    {
        ArgumentNullException.ThrowIfNull(userSession);
        await _userSessionsRepository.Update(userSession);
        var cacheKey = CachingKey.SessionKey(token);
        await _staticCacheManager.RemoveAsync(cacheKey);
        await _staticCacheManager.Set(cacheKey, userSession);
    }

    /// <summary>
    /// Gets a user session by session string
    /// </summary>
    /// <param name="token"></param>
    /// <param name="activeOnly"></param>
    /// <returns></returns>
    public virtual async Task<UserSessions> GetByToken(string token, bool activeOnly = true)
    {
        var cache = await _staticCacheManager.GetOrSetAsync(
            CachingKey.SessionKey(token),
            async () =>
            {
                var hashedToken = token.Hash();
                var query = _userSessionsRepository.Table.Where(s => s.Token == hashedToken);

                if (activeOnly)
                {
                    query = query.Where(s => s.Acttype == "I" || s.Acttype == "S");
                }
                query = from s in query select s;
                var session = await query.OrderByDescending(s => s.Id).FirstOrDefaultAsync();

                return session;
            }
        );
        return cache;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="mac"></param>
    /// <returns></returns>
    public virtual async Task UpdateSessionMac(string token, string mac)
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        userSession.Mac = mac;
        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public virtual async Task UpdateInfo(string token, string info)
    {
        var userSession = await GetByToken(token, false);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        userSession.Info = info;

        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="mac"></param>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    public virtual async Task UpdateSessionMacAndApplicationCode(
        string token,
        string mac,
        string applicationCode
    )
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        userSession.Mac = mac;
        userSession.ApplicationCode = applicationCode;

        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="applicationCode"></param>
    /// <returns></returns>
    public virtual async Task UpdateSessionAndApplicationCode(string token, string applicationCode)
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        userSession.ApplicationCode = applicationCode;

        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task UpdateActtype(
        string token,
        string acttype,
        bool isLockoutOldSession = false
    )
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            if (!isLockoutOldSession)
            {
                throw await nameof(userSession).Required();
            }
            else
            {
                return;
            }
        }
        if (acttype == SessionStatus.Logout)
        {
            await _staticCacheManager.Remove(CachingKey.SessionKey(token));
        }

        userSession.Acttype = acttype;

        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public virtual async Task UpdateSessionAndInfo(string token, string info)
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        userSession.Info = info;

        await _userSessionsRepository.Update(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public virtual async Task DeleteToken(string token)
    {
        var userSession = await GetByToken(token);

        if (userSession == null)
        {
            throw new ArgumentNullException(nameof(userSession));
        }

        await _userSessionsRepository.Delete(userSession);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="applicationCode"></param>
    /// <param name="currentToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllAppToken(
        string userId,
        string applicationCode,
        string currentToken
    )
    {
        Dictionary<string, string> searchInput = new()
        {
            { "Usrid", userId },
            { "ApplicationCode", applicationCode },
            { "Token", currentToken },
        };

        await _userSessionsRepository.FilterAndDelete(searchInput);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllById(string userId)
    {
        Dictionary<string, string> searchInput = new() { { "Usrid", userId } };

        await _userSessionsRepository.FilterAndDelete(searchInput);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="expiretime"></param>
    /// <returns></returns>
    public virtual async Task<UserSessions> GetListUserSessionByLoginNameAndExpireTime(
        string loginName,
        DateTimeOffset expiretime
    )
    {
        return await _userSessionsRepository
            .Table.Where(s =>
                s.Exptime <= expiretime.DateTime
                && s.LoginName == loginName
                && (s.Acttype == "I" || s.Acttype == "L")
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="mac"></param>
    /// <returns></returns>
    public virtual async Task<List<UserSessions>> GetListUserSessionByLoginNameAndNotEqualMac(
        string loginName,
        string mac
    )
    {
        return await _userSessionsRepository
            .Table.Where(s =>
                s.Mac != mac && _jwtTokenService.GetLoginNameFromToken(s.Token, false) == loginName
            )
            .ToListAsync<UserSessions>();
    }

    public async Task<UserSessions> GetActiveByLoginName(string loginName)
    {
        return await _userSessionsRepository
            .Table.Where(s => s.LoginName == loginName && s.Acttype == SessionStatus.Login)
            .OrderByDescending(s => s.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<ValidateSessionModel> CheckValidSingleSession(
        string loginName,
        bool isUpdateOldSession = false
    )
    {
        var session = await GetActiveByLoginName(loginName);

        if (session == null)
        {
            return new ValidateSessionModel();
        }

        var deviceId = session.SessionDevice.GetValueFromJson("device_id");
        if (deviceId.HasValue() && _context.InfoRequest.DeviceID.HasValue())
        {
            if (deviceId == _context.InfoRequest.DeviceID)
            {
                if (isUpdateOldSession && session.Acttype != SessionStatus.Logout)
                {
                    session.Acttype = SessionStatus.Logout;
                    await _userSessionsRepository.Update(session);
                    await _staticCacheManager.Remove(CachingKey.SessionKey(session.Token));
                }
                return new ValidateSessionModel(session);
            }
            else
            {
                throw new O24OpenAPIException(
                    $"This user is currently logged in on device [{session.SessionDevice.GetValueFromJson("device_name")}] [{deviceId}]."
                );
            }
        }

        if (session.Wsip == _context.InfoRequest.GetIp())
        {
            if (isUpdateOldSession && session.Acttype != SessionStatus.Logout)
            {
                session.Acttype = SessionStatus.Logout;
                await _userSessionsRepository.Update(session);
                await _staticCacheManager.Remove(CachingKey.SessionKey(session.Token));
            }
            return new ValidateSessionModel(session);
        }

        return new ValidateSessionModel(session, false);
    }

    public async Task<(bool isValid, UserSessions userSession)> CheckValidSession(string token)
    {
        var userSession = await GetByToken(token);
        if (userSession == null)
        {
            return (false, null);
        }

        return (true, userSession);
    }

    public async Task<(UserSessions, string, string)> RefreshSession(UserSessions userSessions)
    {
        try
        {
            var expireTime = GlobalVariable.ExpireTime();
            var deviceId = userSessions.SessionDevice.GetValueFromJson("device_id");
            var device = JObject.Parse(userSessions.SessionDevice);
            device["my_id"] = _context.InfoRequest.GetIp();
            var token = _jwtTokenService.GetNewJwtToken(
                new User
                {
                    Id = userSessions.Usrid,
                    Username = userSessions.Usrname,
                    UserCode = userSessions.UserCode,
                    BranchCode = userSessions.UsrBranch,
                    LoginName = userSessions.LoginName,
                    DeviceId = deviceId,
                },
                expireTime.ToUnixTimeSeconds()
            );
            var hashedToken = token.Hash();
            var refreshToken = JwtTokenService.GenerateRefreshToken();
            var hashedRefreshToken = refreshToken.Hash();
            // Tạo session mới với token mới
            var newUserSession = new UserSessions(userSessions)
            {
                Acttype = SessionStatus.Login,
                Token = hashedToken,
                Wsname = _context.InfoUser.GetUserLogin().GetDeviceName(),
                SessionDevice = device.ToSerialize(),
                HashedRefreshToken = hashedRefreshToken,
            };

            // Logout session cũ
            if (userSessions.Acttype != SessionStatus.Logout)
            {
                userSessions.Acttype = SessionStatus.Logout;
                await _userSessionsRepository.Update(userSessions);
            }
            await _staticCacheManager.Remove(CachingKey.SessionKey(userSessions.Token));

            // Insert session mới
            var response = await _userSessionsRepository.InsertAsync(newUserSession);
            await _staticCacheManager.Set(
                CachingKey.SessionKey(newUserSession.Token),
                newUserSession
            );

            return (response, token, refreshToken);
        }
        catch
        {
            return (null, null, null);
        }
    }

    public async Task ClearSession(string loginName)
    {
        var activeSessions = await _userSessionsRepository
            .Table.Where(s => s.LoginName == loginName && s.Acttype == "I")
            .ToListAsync();
        if (activeSessions.Count < 1)
        {
            throw new O24OpenAPIException("No active session found");
        }
        foreach (var s in activeSessions)
        {
            await UpdateActtype(s.Token, "O");
        }
    }

    public async Task<UserSessions> GetByRefreshToken(string refreshToken)
    {
        var hashed = refreshToken.Hash();
        return await _userSessionsRepository
            .Table.Where(s => s.HashedRefreshToken == hashed)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateSignatureKey(string token, string signatureKey)
    {
        var session = await GetByToken(token);
        if (session != null)
        {
            session.SignatureKey = signatureKey;
            await UpdateAsync(session, token);
        }
    }
}
