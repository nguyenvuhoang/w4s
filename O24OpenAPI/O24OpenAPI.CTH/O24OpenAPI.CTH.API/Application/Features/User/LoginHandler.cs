using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Configurations;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class LoginCommand : BaseTransactionModel, ICommand<AuthResponseModel>
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

[CqrsHandler]
public class LoginHandler(
    ISupperAdminRepository supperAdminRepository,
    IUserAccountRepository userAccountRepository,
    IJwtTokenService jwtTokenService,
    IUserRightRepository userRightRepository,
    IUserSessionRepository userSessionRepository,
    IUserDeviceRepository userDeviceRepository,
    WebApiSettings webApiSettings,
    IUserPasswordRepository userPasswordRepository,
    IUserRightChannelRepository userRightChannelRepository
) : ICommandHandler<LoginCommand, AuthResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_LOGIN)]
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
        DateTime currentTime = DateTime.UtcNow;
        DateTime expireTime = currentTime.AddDays(
            Convert.ToDouble(webApiSettings.TokenLifetimeDays)
        );
        string token = model.CoreToken;
        string refreshToken = model.RefreshToken;
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
        string hashedToken = token.Hash();
        string hashedRefreshToken = refreshToken.Hash();
        string stringJson = model.RoleChannel;
        int[]? listRoles = System.Text.Json.JsonSerializer.Deserialize<int[]>(stringJson);
        HashSet<string> channelRoles = await userRightChannelRepository.GetSetChannelInRoleAsync(
            listRoles
        );
        await userSessionRepository.RevokeByLoginName(model.LoginName);

        UserSession userSession = new()
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
        DateTime currentTime = DateTime.UtcNow;
        DateTime expireTime = currentTime.AddDays(
            Convert.ToDouble(webApiSettings.TokenLifetimeDays)
        );

        UserAccount userAccount = await GetLoginAccount(
            model.LoginName,
            password: model.Password,
            model.ChannelId,
            model.Language
        );

        await userDeviceRepository.EnsureUserDeviceAsync(
            userCode: userAccount.UserCode,
            loginName: model.LoginName,
            deviceId: (model.DeviceId + model.Modelname) ?? "",
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

        string token = jwtTokenService.GetNewJwtToken(
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
        string hashedToken = token.Hash();
        string refreshToken = JwtTokenService.GenerateRefreshToken();
        string hashedRefreshToken = refreshToken.Hash();
        string stringJson = !string.IsNullOrEmpty(userAccount.RoleChannel)
            ? userAccount.RoleChannel
            : model.RoleChannel;
        int[]? listRoles = System.Text.Json.JsonSerializer.Deserialize<int[]>(stringJson);
        HashSet<string> channelRoles = await userRightChannelRepository.GetSetChannelInRoleAsync(
            listRoles
        );
        await userSessionRepository.RevokeByLoginName(model.LoginName);

        UserSession userSession = new()
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
            Device = (model.DeviceId + model.Modelname) ?? "",
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
        SupperAdmin sAdmin =
            await supperAdminRepository.IsExit()
            ?? throw new O24OpenAPIException("Supper Admin does not exit.");

        if (sAdmin.LoginName != request.LoginName)
        {
            throw new O24OpenAPIException("Supper Admin does not exit or invalid login name.");
        }
        ;

        UserAccount userAccount = await GetLoginAccount(
            request.LoginName,
            password: request.Password,
            request.ChannelId,
            request.Language
        );

        DateTime currentTime = DateTime.UtcNow;
        DateTime expireTime = currentTime.AddDays(
            Convert.ToDouble(webApiSettings.TokenLifetimeDays)
        );

        string token = jwtTokenService.GetNewJwtToken(
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

        string hashedToken = token.Hash();
        string refreshToken = JwtTokenService.GenerateRefreshToken();
        string hashedRefreshToken = refreshToken.Hash();
        HashSet<string> channelRoles = await userRightChannelRepository.GetSetChannelInRoleAsync(1);
        UserSession userSession = new()
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
            Device = (request.DeviceId + request.Modelname) ?? "",
        };
        await userSessionRepository.Insert(userSession);

        await userDeviceRepository.EnsureUserDeviceAsync(
           userCode: userAccount.UserCode,
           loginName: request.LoginName,
           deviceId: (request.DeviceId + request.Modelname) ?? "",
           deviceType: request.DeviceType,
           userAgent: request.UserAgent,
           ipAddress: request.IpAddress,
           channelId: request.ChannelId,
           pushId: request.PushId,
           osVersion: request.OsVersion,
           appVersion: request.AppVersion,
           deviceName: request.DeviceName,
           brand: request.Brand,
           isEmulator: request.IsEmulator,
           isRooted: request.IsRootedOrJailbroken,
           language: request.Language,
           isResetDevice: request.IsResetDevice,
           network: request.Network,
           memory: request.Memory
       );


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

    private async Task<UserAccount> GetLoginAccount(
        string loginName,
        string password,
        string channelId,
        string language
    )
    {
        UserAccount user =
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

        UserPassword userPassword =
            await userPasswordRepository
                .Table.Where(s => s.ChannelId == channelId && s.UserId == user.UserId)
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PasswordDonotSetting,
                language,
                []
            );

        ControlHubSetting? setting = EngineContext.Current.Resolve<ControlHubSetting>();

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
