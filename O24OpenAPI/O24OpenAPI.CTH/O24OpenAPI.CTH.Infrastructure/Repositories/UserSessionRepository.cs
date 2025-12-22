using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserSessionRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserSession>(eventPublisher, dataProvider, staticCacheManager),
        IUserSessionRepository
{
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

    public async Task Insert(UserSession userSession)
    {
        //ArgumentNullException.ThrowIfNull(userSession);
        await InsertAsync(userSession);
        // var sessionModel = userSession.ToModel<UserSessionModel>();
        await staticCacheManager.Set(CachingKey.SessionKey(userSession.Token), userSession);
    }

    public Task RevokeByLoginName(string loginName)
    {
        throw new NotImplementedException();
    }
}
