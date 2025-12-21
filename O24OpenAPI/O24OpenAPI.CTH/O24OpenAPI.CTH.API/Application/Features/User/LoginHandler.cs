using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Configurations;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class LoginCommand : BaseTransactionModel, ICommand<LoginToO24OpenAPIRequestModel>
{
    public string Reference { get; set; }

    public string UserId { get; set; }

    public string LoginName { get; set; }

    public string UserCode { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }
    public string BranchCode { get; set; }

    public bool IsSupperAdmin { get; set; }

    public string DeviceId { get; set; } = string.Empty;

    public string DeviceType { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    public string RoleChannel { get; set; }
    public bool IsO24ManageUser { get; set; } = true;

    public string PushId { get; set; }

    public string OsVersion { get; set; }

    public string AppVersion { get; set; }

    public string DeviceName { get; set; }

    public string Brand { get; set; }

    public bool IsEmulator { get; set; }

    public bool IsRootedOrJailbroken { get; set; }

    public string Modelname { get; set; } = string.Empty;

    public bool IsResetDevice { get; set; } = false;

    public string CoreToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string Network { get; set; } = string.Empty;

    public string Memory { get; set; } = string.Empty;
}

public class LoginHandel(
    IAuthenticateRepository authenticateRepository,
    ISupperAdminRepository supperAdminRepository,
    IUserAccountRepository userAccountRepository,
    IJwtTokenService jwtTokenService,
    IUserRightRepository userRightRepository,
    IUserSessionRepository userSessionRepository,
    IUserDeviceRepository userDeviceRepository,
    WebApiSettings webApiSettings,
    IUserPasswordRepository userPasswordRepository
) : ICommandHandler<LoginCommand, LoginToO24OpenAPIRequestModel>
{
    public async Task<AuthResponseModel> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.LoginName == "sadmin")
        {
            return await LoginBySupperAdmin(request);
        }
        if (request.IsO24ManageUser)
        {
            return await LoginByO24User(request);
        }
        return await GenerateTokenAndSession(request);
    }

    private async Task<AuthResponseModel> GenerateTokenAndSession(LoginCommand model)
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(webApiSettings.TokenLifetimeDays));
        var token = model.CoreToken;
        var refreshToken = model.RefreshToken;
        if (string.IsNullOrEmpty(token))
        {
            token = jwtTokenService.GetNewJwtToken(
                new O24OpenAPI.Core.Domain.Users.User
                {
                    Id = int.Parse(model.UserId),
                    Username = model.LoginName,
                    UserCode = model.UserId,
                    BranchCode = model.BranchCode,
                    LoginName = model.LoginName,
                    DeviceId = model.DeviceId,
                },
                ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
            );
        }
        if (string.IsNullOrEmpty(refreshToken))
        {
            refreshToken = JwtTokenService.GenerateRefreshToken();
        }
        var hashedToken = token.Hash();
        var hashedRefreshToken = refreshToken.Hash();
        var stringJson = model.RoleChannel;
        var listRoles = System.Text.Json.JsonSerializer.Deserialize<int[]>(stringJson);
        var channelRoles = await userRightRepository.GetSetChannelInRoleAsync(listRoles);
        await userSessionRepository.RevokeByLoginName(model.LoginName);

        var userSession = new UserSession
        {
            Token = hashedToken,
            UserId = model.UserId,
            LoginName = model.LoginName,
            Reference = model.Reference,
            IpAddress = model.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = model.ChannelId,
            RefreshToken = hashedRefreshToken,
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = channelRoles.ToSerializeSystemText(),
            UserCode = model.UserId,
            BranchCode = model.BranchCode,
            UserName = model.UserName,
        };

        await userSessionRepository.Insert(userSession);

        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
        };
    }

    public async Task<AuthResponseModel> LoginByO24User(LoginCommand model)
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(webApiSettings.TokenLifetimeDays));

        var userAccount = await GetLoginAccount(
            model.LoginName,
            password: model.Password,
            model.ChannelId,
            model.Language
        );

        await userDeviceRepository.EnsureUserDeviceAsync(
            userCode: userAccount.UserCode,
            loginName: model.LoginName,
            deviceId: model.DeviceId + model.Modelname ?? "",
            deviceType: model.DeviceType,
            userAgent: model.UserAgent,
            ipAddress: model.IpAddress,
            channelId: model.ChannelId,
            pushId: model.PushId,
            osVersion: model.OsVersion,
            appVersion: model.AppVersion,
            deviceName: model.DeviceName,
            brand: model.Brand,
            isEmulator: model.IsEmulator,
            isRooted: model.IsRootedOrJailbroken,
            language: model.Language,
            isResetDevice: model.IsResetDevice,
            network: model.Network,
            memory: model.Memory
        );

        var token = jwtTokenService.GetNewJwtToken(
            new O24OpenAPI.Core.Domain.Users.User
            {
                Id = userAccount.Id,
                Username = userAccount.UserName,
                UserCode = userAccount.UserCode,
                BranchCode = userAccount.BranchID,
                LoginName = userAccount.LoginName,
                DeviceId = model.DeviceId,
            },
            ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
        );
        var hashedToken = token.Hash();
        var refreshToken = JwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = refreshToken.Hash();
        var stringJson = !string.IsNullOrEmpty(userAccount.RoleChannel)
            ? userAccount.RoleChannel
            : model.RoleChannel;
        var listRoles = System.Text.Json.JsonSerializer.Deserialize<int[]>(stringJson);
        var channelRoles = await userRightRepository.GetSetChannelInRoleAsync(listRoles);
        await userSessionRepository.RevokeByLoginName(model.LoginName);

        var userSession = new UserSession
        {
            Token = hashedToken,
            UserId = userAccount.UserId,
            LoginName = model.LoginName,
            Reference = model.Reference,
            IpAddress = model.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = model.ChannelId,
            RefreshToken = hashedRefreshToken,
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = channelRoles.ToSerializeSystemText(),
            UserCode = userAccount.UserCode,
            BranchCode = userAccount.BranchCode,
            UserName = userAccount.UserName,
            Device = model.DeviceId + model.Modelname ?? "",
        };
        await userSessionRepository.Insert(userSession);

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = $"{Guid.NewGuid()}";
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;
        await userAccountRepository.UpdateAsync(userAccount);

        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
            IsFirstLogin = userAccount.IsFirstLogin,
        };
    }

    private async Task<AuthResponseModel> LoginBySupperAdmin(LoginCommand request)
    {
        var sAdmin =
            await supperAdminRepository.IsExit()
            ?? throw new O24OpenAPIException("Supper Admin does not exit.");

        if (sAdmin.UserId != request.UserId || sAdmin.LoginName != request.LoginName)
        {
            throw new O24OpenAPIException("Supper Admin does not exit or invalid login name.");
        }
        ;

        var userAccount = await GetLoginAccount(
            request.LoginName,
            password: request.Password,
            request.ChannelId,
            request.Language
        );

        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(webApiSettings.TokenLifetimeDays));

        var token = jwtTokenService.GetNewJwtToken(
            new O24OpenAPI.Core.Domain.Users.User
            {
                Id = userAccount.Id,
                LoginName = userAccount.LoginName,
                UserCode = userAccount.UserCode,
                BranchCode = userAccount.BranchID,
                Username = userAccount.UserName,
                DeviceId = request.DeviceId,
            },
            ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
        );

        var hashedToken = token.Hash();
        var refreshToken = JwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = refreshToken.Hash();
        var channelRoles = await userRightRepository.GetSetChannelInRoleAsync(1);
        var userSession = new UserSession
        {
            Token = hashedToken,
            UserId = userAccount.UserId,
            LoginName = request.LoginName,
            Reference = request.Reference,
            IpAddress = request.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = request.ChannelId,
            RefreshToken = hashedRefreshToken,
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = channelRoles.ToSerializeSystemText(),
            UserCode = userAccount.UserCode,
            BranchCode = userAccount.BranchCode,
            UserName = userAccount.UserName,
            Device = request.DeviceId + request.Modelname ?? "",
        };
        await userSessionRepository.Insert(userSession);
        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = $"{Guid.NewGuid()}";
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;
        await userAccountRepository.UpdateAsync(userAccount);
        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
        };
    }

    Task<LoginToO24OpenAPIRequestModel> IHandler<
        LoginCommand,
        LoginToO24OpenAPIRequestModel
    >.HandleAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<UserAccount> GetLoginAccount(
        string loginName,
        string password,
        string channelId,
        string language
    )
    {
        var user =
            await userAccountRepository
                .Table.Where(s =>
                    s.ChannelId == channelId
                    && s.LoginName == loginName
                    && s.Status != Common.DELETED
                )
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                language,
                [loginName]
            );

        var userPassword =
            await userPasswordRepository
                .Table.Where(s => s.ChannelId == channelId && s.UserId == user.UserId)
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordDonotSetting,
                language,
                []
            );

        var setting = EngineContext.Current.Resolve<ControlHubSetting>();

        if (user.Status == Common.BLOCK && user.LockedUntil.HasValue)
        {
            if (user.LockedUntil > DateTime.UtcNow)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.AccountLockedTemporarily,
                    language,
                    [loginName, user.LockedUntil.Value.ToString("HH:mm:ss")]
                );
            }
            else
            {
                user.Status = Common.ACTIVE;
                user.Failnumber = 0;
                user.LockedUntil = null;
                await userAccountRepository.UpdateAsync(user);
            }
        }

        bool isPasswordValid;
        try
        {
            isPasswordValid = PasswordUtils.VerifyPassword(
                usercode: user.UserCode,
                password: password,
                storedHash: userPassword.Password,
                storedSalt: userPassword.Salt
            );
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            isPasswordValid = false;
        }

        if (!isPasswordValid)
        {
            user.Failnumber++;

            if (user.Failnumber >= setting.MaxFailedAttempts)
            {
                user.Status = Common.BLOCK;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                await userAccountRepository.UpdateAsync(user);

                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.AccountLockedTemporarily,
                    language,
                    [loginName, user.LockedUntil.Value.ToString("HH:mm:ss")]
                );
            }

            await userAccountRepository.UpdateAsync(user);

            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordIncorrect,
                language,
                [user.Failnumber]
            );
        }

        user.Failnumber = 0;
        user.LockedUntil = null;
        await userAccountRepository.UpdateAsync(user);

        if (user.Status != Common.ACTIVE)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.AccountStatusInvalid,
                language,
                [user.LoginName]
            );
        }

        return user;
    }
}
