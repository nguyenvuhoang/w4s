using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserSessionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserSession>(dataProvider, staticCacheManager), IUserSessionRepository
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public async Task<UserSession?> GetByHashedTokenAsync(
        string hashedToken,
        bool activeOnly = true
    )
    {
        IQueryable<UserSession> query = Table.Where(s => s.Token == hashedToken);

        if (activeOnly)
        {
            query = query.Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserSession?> GetActiveByLoginNameAsync(string loginName)
    {
        return await Table
            .Where(x => x.LoginName == loginName && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserSession>> GetNotRevokedByLoginNameAsync(string loginName)
    {
        return await Table.Where(x => x.LoginName == loginName && !x.IsRevoked).ToListAsync();
    }

    public async Task<UserSession?> GetByRefreshTokenAsync(string hashedRefreshToken)
    {
        return await Table.Where(s => s.RefreshToken == hashedRefreshToken).FirstOrDefaultAsync();
    }

    public async Task AddAsync(UserSession userSession)
    {
        //ArgumentNullException.ThrowIfNull(userSession);
        await InsertAsync(userSession);
        // var sessionModel = userSession.ToModel<UserSessionModel>();
        await _staticCacheManager.Set(CachingKey.SessionKey(userSession.Token!), userSession);
    }

    public async Task RevokeByLoginName(string loginName)
    {
        List<UserSession> sessions = await Table
            .Where(x => x.LoginName == loginName && !x.IsRevoked)
            .ToListAsync();

        foreach (UserSession? session in sessions)
        {
            session.IsRevoked = true;
            session.ExpiresAt = DateTime.UtcNow;
            await Update(session);
            await _staticCacheManager.Remove(CachingKey.SessionKey(session.Token));
        }
    }

    public async Task<UserSession?> GetByRefreshToken(string token)
    {
        string hashed = token.Hash();
        return await Table.Where(s => s.RefreshToken == hashed).FirstOrDefaultAsync();
    }

    public async Task<UserSession?> GetActiveByLoginName(string loginName)
    {
        return await Table
            .Where(x => x.LoginName == loginName && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<UserSession?> GetByToken(string token, bool activeOnly = true)
    {
        CacheKey cacheKey = CachingKey.SessionKey(token);
        UserSession? session = await _staticCacheManager.Get<UserSession>(cacheKey);

        if (session == null || session.Id == 0)
        {
            string hashedToken = token.Hash();
            IQueryable<UserSession> query = Table.Where(s => s.Token == hashedToken);

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

    public async Task UpdateSignatureKey(string token, string signatureKey)
    {
        UserSession? session = await GetByToken(token);
        if (session != null)
        {
            session.SignatureKey = signatureKey;
            await Update(session);
            await _staticCacheManager.Remove(CachingKey.SessionKey(token));
        }
    }
}
