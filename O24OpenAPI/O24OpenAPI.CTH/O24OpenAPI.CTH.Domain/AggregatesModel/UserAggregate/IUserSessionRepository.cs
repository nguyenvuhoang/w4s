using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserSessionRepository : IRepository<UserSession>
{
    Task Insert(UserSession userSession);
    Task RevokeByLoginName(string loginName);
    Task<UserSession?> GetByRefreshToken(string token);
    Task<UserSession?> GetActiveByLoginName(string loginName);
    Task<UserSession?> GetByToken(string token, bool activeOnly = true);
}
