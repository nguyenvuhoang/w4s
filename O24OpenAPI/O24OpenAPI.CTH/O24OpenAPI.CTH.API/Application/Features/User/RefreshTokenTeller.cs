using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class RefreshTokenTellerCommand : BaseTransactionModel, ICommand<AuthResponseModel>
{
    public string CoreToken { get; set; }
    public string RefreshToken { get; set; }
    public string OldRefreshToken { get; set; }
}

[CqrsHandler]
public class RefreshTokenTellerHandle(
    IUserSessionRepository userSessionRepository,
    WebApiSettings webApiSettings,
    IStaticCacheManager staticCacheManager
) : ICommandHandler<RefreshTokenTellerCommand, AuthResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_UMG_REFRESH_TOKEN)]
    public async Task<AuthResponseModel> HandleAsync(
        RefreshTokenTellerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var userSessions =
            await userSessionRepository.GetByRefreshToken(request.OldRefreshToken)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.InvalidSessionRefresh,
                request.Language,
                request.CurrentUserCode
            );

        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(webApiSettings.TokenLifetimeDays));

        var token = request.CoreToken;

        var refreshToken = request.RefreshToken;

        if (string.IsNullOrEmpty(refreshToken))
        {
            refreshToken = JwtTokenService.GenerateRefreshToken();
        }

        await Revoke(request.Token);

        var userSession = new UserSession
        {
            Token = token.Hash(),
            UserId = userSessions.UserId,
            LoginName = userSessions.LoginName,
            Reference = userSessions.Reference,
            IpAddress = userSessions.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = request.ChannelId,
            RefreshToken = refreshToken.Hash(),
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = userSessions.ChannelRoles,
            UserCode = userSessions.UserCode,
            BranchCode = userSessions.BranchCode,
            UserName = userSessions.UserName,
        };

        await userSessionRepository.Insert(userSession);

        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }

    public virtual async Task Revoke(string token)
    {
        var userSession =
            await GetByToken(token) ?? throw new O24OpenAPIException("Invalid session.");

        userSession.Revoke();
        await staticCacheManager.Remove(new CacheKey(token));

        await userSessionRepository.Update(userSession);
    }

    public virtual async Task<UserSession> GetByToken(string token, bool activeOnly = true)
    {
        var cacheKey = new CacheKey(token);
        var session = await staticCacheManager.Get<UserSession>(cacheKey);

        if (session == null || session.Id == 0)
        {
            var hashedToken = token.Hash();
            var query = userSessionRepository.Table.Where(s => s.Token == hashedToken);

            if (activeOnly)
            {
                query = query.Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
            }

            session = await query.FirstOrDefaultAsync();

            if (session != null)
            {
                await staticCacheManager.Set(cacheKey, session);
            }
        }

        return session;
    }
}
