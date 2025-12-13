using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Exceptions;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// The user session service class
/// </summary>
/// <seealso cref="IUserSessionService"/>
public partial class UserSessionService(
    IRepository<UserSession> userSessionRepo,
    IStaticCacheManager staticCacheManager
) : IUserSessionService
{
    /// <summary>
    /// The user session repo
    /// </summary>
    private readonly IRepository<UserSession> _userSessionRepo = userSessionRepo;

    /// <summary>
    /// The static cache manager
    /// </summary>
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    /// <summary>
    /// Add a new session
    /// </summary>
    /// <param name="userSession"></param>
    /// <returns></returns>
    public virtual async Task Insert(UserSession userSession)
    {
        ArgumentNullException.ThrowIfNull(userSession);
        await _userSessionRepo.Insert(userSession);
        // var sessionModel = userSession.ToModel<UserSessionModel>();
        await _staticCacheManager.Set(CachingKey.SessionKey(userSession.Token), userSession);
    }

    public async Task UpdateAsync(UserSession userSession, string token)
    {
        ArgumentNullException.ThrowIfNull(userSession);
        await _userSessionRepo.Update(userSession);
        var cacheKey = CachingKey.SessionKey(token);
        await _staticCacheManager.RemoveAsync(cacheKey);
        await _staticCacheManager.Set(cacheKey, userSession);
    }

    /// <summary>
    /// Gets a user session by token
    /// </summary>
    /// <param name="token"></param>
    /// <param name="activeOnly"></param>
    /// <returns></returns>
    public virtual async Task<UserSession> GetByToken(string token, bool activeOnly = true)
    {
        var cacheKey = new CacheKey(token);
        var session = await _staticCacheManager.Get<UserSession>(cacheKey);

        if (session == null || session.Id == 0)
        {
            var hashedToken = token.Hash();
            var query = _userSessionRepo.Table.Where(s => s.Token == hashedToken);

            if (activeOnly)
            {
                query = query.Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
            }

            session = await query.FirstOrDefaultAsync();

            if (session != null)
            {
                await _staticCacheManager.Set(cacheKey, session);
            }
        }

        return session;
    }

    /// <summary>
    /// Revoke token
    /// </summary>
    /// <returns></returns>
    public virtual async Task Revoke(string token)
    {
        var userSession =
            await GetByToken(token) ?? throw new O24OpenAPIException("Invalid session.");

        userSession.Revoke();
        await _staticCacheManager.Remove(new CacheKey(token));

        await _userSessionRepo.Update(userSession);
    }

    /// <summary>
    /// Delete a token
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

        await _staticCacheManager.Remove(new CacheKey(token));

        await _userSessionRepo.Delete(userSession);
    }

    /// <summary>
    /// Delete all channel's token
    /// </summary>
    /// <returns></returns>
    public virtual async Task DeleteChannelToken(string channelId)
    {
        Dictionary<string, string> searchInput = new() { { "ChannelId", channelId } };

        await _userSessionRepo.FilterAndDelete(searchInput);
    }

    /// <summary>
    /// Delete all user's token
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllByUserId(string userId)
    {
        Dictionary<string, string> searchInput = new() { { "UserId", userId } };

        await _userSessionRepo.FilterAndDelete(searchInput);
    }

    /// <summary>
    /// Gets the active by login name using the specified login name
    /// </summary>
    /// <param name="loginName">The login name</param>
    /// <returns>A task containing the user session</returns>
    public async Task<UserSession> GetActiveByLoginName(string loginName)
    {
        return await _userSessionRepo
            .Table.Where(x =>
                x.LoginName == loginName && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Checks the valid single session using the specified login name
    /// </summary>
    /// <param name="loginName">The login name</param>
    /// <param name="isUpdateOldSession">The is update old session</param>
    /// <returns>A task containing the validate session model</returns>
    public async Task<ValidateSessionModel> CheckValidSingleSession(
        string loginName,
        string language = "en"
    )
    {
        var session =
            await GetActiveByLoginName(loginName)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.InvalidSessionStatus,
                language,
                [loginName]
            );

        var sessionModel = new ValidateSessionModel(
            isValidSession: true,
            deviceName: session.Device
        );

        return sessionModel;
    }

    public async Task RevokeByLoginName(string loginName)
    {
        var sessions = await _userSessionRepo
            .Table.Where(x => x.LoginName == loginName && !x.IsRevoked)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.IsRevoked = true;
            session.ExpiresAt = DateTime.UtcNow;
            await _userSessionRepo.Update(session);
            await _staticCacheManager.Remove(new CacheKey(session.Token));
        }
    }

    public async Task<UserSession> GetByRefreshToken(string token)
    {
        var hashed = token.Hash();
        return await _userSessionRepo
            .Table.Where(s => s.RefreshToken == hashed)
            .FirstOrDefaultAsync();
    }
}
