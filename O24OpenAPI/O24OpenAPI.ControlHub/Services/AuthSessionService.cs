using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Domain.Users;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.Framework;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.ControlHub.Services;

public class AuthSessionService(
    IJwtTokenService jwtTokenService,
    IUserSessionService userSessionService,
    IUserRightService userRightService,
    WebApiSettings setting
) : IAuthSessionService
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserSessionService _userSessionService = userSessionService;
    private readonly IUserRightService _userRightService = userRightService;
    private readonly WebApiSettings _setting = setting;

    public async Task<AuthResponseModel> CreateTokenAndSessionAsync(
        UserAccount userAccount,
        LoginContextModel context
    )
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(_setting.TokenLifetimeDays));

        var token = _jwtTokenService.GetNewJwtToken(
            new User
            {
                Id = userAccount.Id,
                Username = userAccount.UserName,
                UserCode = userAccount.UserCode,
                BranchCode = userAccount.BranchID,
                LoginName = userAccount.LoginName,
                DeviceId = context.DeviceId,
            },
            ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
        );

        var refreshToken = JwtTokenService.GenerateRefreshToken();
        var hashedToken = token.Hash();
        var hashedRefreshToken = refreshToken.Hash();

        var listRoles = System.Text.Json.JsonSerializer.Deserialize<int[]>(context.RoleChannel);
        var channelRoles = await _userRightService.GetSetChannelInRoleAsync(listRoles);

        await _userSessionService.RevokeByLoginName(userAccount.LoginName);

        var session = new UserSession
        {
            Token = hashedToken,
            UserId = userAccount.UserId,
            LoginName = userAccount.LoginName,
            Reference = context.Reference,
            IpAddress = context.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = context.ChannelId,
            RefreshToken = hashedRefreshToken,
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = channelRoles.ToSerializeSystemText(),
            UserCode = userAccount.UserCode,
            BranchCode = userAccount.BranchID,
            UserName = userAccount.UserName,
            Device = context.DeviceId + context.Modelname ?? "",
        };

        await _userSessionService.Insert(session);
        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }
}
