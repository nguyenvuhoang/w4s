using LinKit.Core.Cqrs;
using LinqToDB;
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

public class RefreshTokenAsyncCommand : BaseTransactionModel, ICommand<AuthResponseModel>
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class RefreshTokenAsyncHandle(
    IUserSessionRepository userSessionRepository,
    IUserAccountRepository userAccountRepository,
    WebApiSettings webApiSettings,
    IJwtTokenService jwtTokenService,
    IStaticCacheManager staticCacheManager
) : ICommandHandler<RefreshTokenAsyncCommand, AuthResponseModel>
{
    [WorkflowStep("WF_STEP_CTH_REFESH_TOKEN")]
    public async Task<AuthResponseModel> HandleAsync(
        RefreshTokenAsyncCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var userSessions =
            await userSessionRepository.GetByRefreshToken(request.RefreshToken)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.InvalidSessionRefresh,
                request.Language,
                request.CurrentUserCode
            );

        var loginName = userSessions.LoginName;

        var validateSessionModel = await CheckValidSingleSession(loginName, request.Language);
        if (!validateSessionModel.IsValid)
        {
            throw await O24Exception.CreateWithNextActionAsync(
                O24CTHResourceCode.Operation.InvalidSessionStatus,
                $"LOGIN",
                request.Language,
                [loginName]
            );
        }

        var userAccount = await userAccountRepository.GetByLoginNameAsync(loginName);
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(
            Convert.ToDouble(webApiSettings.TokenLifetimeDays)
        );

        if (userAccount.Status != Common.ACTIVE)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.AccountStatusInvalid,
                request.Language,
                [loginName]
            );
        }

        var token = jwtTokenService.GetNewJwtToken(
            new O24OpenAPI.Core.Domain.Users.User
            {
                Id = userAccount.Id,
                Username = userAccount.UserName,
                UserCode = userAccount.UserCode,
                BranchCode = userAccount.BranchID,
                LoginName = userAccount.LoginName,
                DeviceId = request.DeviceId,
            },
            ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
        );

        var refreshToken = JwtTokenService.GenerateRefreshToken();

        await RevokeByLoginName(loginName);

        var userSession = new UserSession
        {
            Token = token.Hash(),
            UserId = userAccount.UserId,
            LoginName = loginName,
            Reference = userSessions.Reference,
            IpAddress = userSessions.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = request.ChannelId,
            RefreshToken = refreshToken.Hash(),
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = userSessions.ChannelRoles,
            UserCode = userAccount.UserCode,
            BranchCode = userAccount.BranchID,
            UserName = userAccount.UserName,
        };

        await userSessionRepository.Insert(userSession);

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = Guid.NewGuid().ToString();
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;

        await userAccountRepository.UpdateAsync(userAccount);

        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }

    public async Task<ValidateSessionModel> CheckValidSingleSession(
        string loginName,
        string language = "en"
    )
    {
        var session =
            await userSessionRepository.GetActiveByLoginName(loginName)
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
        var sessions = await userSessionRepository
            .Table.Where(x => x.LoginName == loginName && !x.IsRevoked)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.IsRevoked = true;
            session.ExpiresAt = DateTime.UtcNow;
            await userSessionRepository.Update(session);
            await staticCacheManager.Remove(new CacheKey(session.Token));
        }
    }
}
