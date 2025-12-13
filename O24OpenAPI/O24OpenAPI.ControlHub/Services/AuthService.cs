using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.ControlHub.Constant;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Request;
using O24OpenAPI.ControlHub.Models.Response;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.ControlHub.Utils;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain.Users;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.EventBus.Abstractions;
using O24OpenAPI.Web.Framework;
using O24OpenAPI.Web.Framework.Constants;
using O24OpenAPI.Web.Framework.Exceptions;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Services;
using O24OpenAPI.Web.Framework.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using StringExtensions = O24OpenAPI.ControlHub.Utils.StringExtensions;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// The auth service class
/// </summary>
/// <seealso cref="IAuthService"/>
public class AuthService(
    IJwtTokenService jwtTokenService,
    IUserAccountService userAccountService,
    WebApiSettings setting,
    IUserSessionService userSessionService,
    ISupperAdminService supperAdminService,
    IUserRightService userRightService,
    IUserCommandService userCommandService,
    IUserDeviceService userDeviceService,
    IUserPasswordService userPasswordService,
    IUserAuthenService userAuthenService,
    IUserAvatarService userAvatarService,
    IHttpContextAccessor httpContextAccessor,
    IUserInRoleService userInRoleService,
    INotificationBuilder notificationBuilder,
    IUserRoleService userRoleService,
    IUserBannerService userBannerService
) : IAuthService
{
    /// <summary>
    /// The setting
    /// </summary>
    private readonly WebApiSettings _setting = setting;

    /// <summary>
    /// The jwt token service
    /// </summary>
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    /// <summary>
    /// The user account service
    /// </summary>
    private readonly IUserAccountService _userAccountService = userAccountService;

    /// <summary>
    /// The user session service
    /// </summary>
    private readonly IUserSessionService _userSessionService = userSessionService;

    /// <summary>
    /// The supper admin service
    /// </summary>
    private readonly ISupperAdminService _supperAdminService = supperAdminService;

    /// <summary>
    /// The user right service
    /// </summary>
    private readonly IUserRightService _userRightService = userRightService;

    /// <summary>
    /// UserCommandService
    /// </summary>
    private readonly IUserCommandService _userCommandService = userCommandService;

    /// <summary>
    /// UserDeviceService
    /// </summary>
    private readonly IUserDeviceService _userDeviceService = userDeviceService;

    /// <summary>
    /// UserPasswordService
    /// </summary>
    private readonly IUserPasswordService _userPasswordService = userPasswordService;

    /// <summary>
    /// UserAuthen
    /// </summary>
    private readonly IUserAuthenService _userAuthenService = userAuthenService;

    /// <summary>
    /// UserAuthen
    /// </summary>
    private readonly IUserAvatarService _userAvatarService = userAvatarService;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly IUserInRoleService _userInRoleService = userInRoleService;

    /// <summary>
    /// The notification builder
    /// </summary>
    private readonly INotificationBuilder _notificationBuilder = notificationBuilder;

    /// <summary>
    /// User Role Service
    /// </summary>
    private readonly IUserRoleService _userRoleService = userRoleService;
    /// <summary>
    /// User Banner Service
    /// </summary>
    private readonly IUserBannerService _userBannerService = userBannerService;

    /// <summary>
    /// Authenticates the model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the auth response model</returns>
    public async Task<AuthResponseModel> Authenticate(LoginToO24OpenAPIRequestModel model)
    {
        if (model.LoginName == "sadmin")
        {
            return await LoginBySupperAdmin(model);
        }
        if (model.IsO24ManageUser)
        {
            return await LoginByO24User(model);
        }
        return await GenerateTokenAndSession(model);
    }

    /// <summary>
    /// LoginByO24User
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<AuthResponseModel> LoginByO24User(LoginToO24OpenAPIRequestModel model)
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(_setting.TokenLifetimeDays));

        var userAccount = await _userAccountService.GetLoginAccount(
            model.LoginName,
            password: model.Password,
            model.ChannelId,
            model.Language
        );

        await _userDeviceService.EnsureUserDeviceAsync(
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

        var token = _jwtTokenService.GetNewJwtToken(
            new User
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
        var channelRoles = await _userRightService.GetSetChannelInRoleAsync(listRoles);
        await _userSessionService.RevokeByLoginName(model.LoginName);

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
        await _userSessionService.Insert(userSession);

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = $"{Guid.NewGuid()}";
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;
        await _userAccountService.UpdateAsync(userAccount);

        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
            IsFirstLogin = userAccount.IsFirstLogin
        };
    }

    /// <summary>
    /// Logins the by supper admin using the specified model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="O24OpenAPIException"></exception>
    public async Task<AuthResponseModel> LoginBySupperAdmin(LoginToO24OpenAPIRequestModel model)
    {
        var sAdmin =
            await _supperAdminService.IsExit()
            ?? throw new O24OpenAPIException("Supper Admin does not exit.");

        if (sAdmin.UserId != model.UserId || sAdmin.LoginName != model.LoginName)
        {
            throw new O24OpenAPIException("Supper Admin does not exit or invalid login name.");
        }
        ;

        var userAccount = await _userAccountService.GetLoginAccount(
            model.LoginName,
            password: model.Password,
            model.ChannelId,
            model.Language
        );

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
                DeviceId = model.DeviceId,
            },
            ((DateTimeOffset)expireTime).ToUnixTimeSeconds()
        );

        var hashedToken = token.Hash();
        var refreshToken = JwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = refreshToken.Hash();
        var channelRoles = await _userRightService.GetSetChannelInRoleAsync(1);
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
        await _userSessionService.Insert(userSession);
        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = $"{Guid.NewGuid()}";
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;
        await _userAccountService.UpdateAsync(userAccount);
        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
        };
    }

    private async Task<AuthResponseModel> GenerateTokenAndSession(
        LoginToO24OpenAPIRequestModel model
    )
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(_setting.TokenLifetimeDays));
        var token = model.CoreToken;
        var refreshToken = model.RefreshToken;
        if (string.IsNullOrEmpty(token))
        {
            token = _jwtTokenService.GetNewJwtToken(
                new User
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
        var channelRoles = await _userRightService.GetSetChannelInRoleAsync(listRoles);
        await _userSessionService.RevokeByLoginName(model.LoginName);

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

        await _userSessionService.Insert(userSession);

        return new AuthResponseModel
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiredIn = expireTime,
            ExpiredDuration = (long)(expireTime - DateTime.Now).TotalSeconds,
        };
    }

    /// <summary>
    /// Creates the supper admin using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="O24OpenAPIException">Supper Admin already exits.</exception>
    /// <returns>A task containing the bool</returns>
    public async Task<bool> CreateSupperAdmin(LoginToO24OpenAPIRequestModel model)
    {
        var exited = await _supperAdminService.IsExit();
        if (exited != null)
        {
            throw new O24OpenAPIException("Supper Admin already exits.");
        }

        var hashedPass = PasswordUtils.HashPass(model.Password);
        var userId = Guid.NewGuid().ToString();
        var sAdmin = new SupperAdmin
        {
            UserId = userId,
            LoginName = model.LoginName,
            PasswordHash = hashedPass,
        };
        var userAccount = new UserAccount()
        {
            ChannelId = ChannelId.Portal,
            UserId = userId,
            UserName = model.LoginName,
            UserCode = Guid.NewGuid().ToString(),
            LoginName = model.LoginName,
            RoleChannel = "[1]",
            Status = "A",
            UserCreated = model.LoginName,
            IsSuperAdmin = true,
            BranchID = "0",
        };
        await _userAccountService.AddAsync(userAccount);
        await _supperAdminService.AddAsync(sAdmin);
        return true;
    }

    /// <summary>
    /// Validates the token using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <returns>The jwt security token</returns>
    public JwtSecurityToken ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_setting.SecretKey);

            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                },
                out var validatedToken
            );

            Console.WriteLine("Token is valid!");
            return (JwtSecurityToken)validatedToken;
        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }

    public async Task<ApplicationInfoResponseModel> ApplicationInfo(ApplicationInfoModel model)
    {
        var listRoleofUser = await _userAccountService.ListOfRole(model.CurrentUserCode);

        var allMenus = new List<CommandHierarchyModel>();

        foreach (var role in listRoleofUser)
        {
            var menus = await _userCommandService.GetMenuInfoFromRoleId(
                role.RoleId,
                model.ChannelId,
                model.Language ?? "en"
            );
            allMenus.AddRange(menus);
        }

        var uniqueMenus = allMenus.GroupBy(m => m.CommandId).Select(g => g.First()).ToList();

        var menuHierarchy = uniqueMenus
            .Where(m => m.ParentId == "0")
            .Select(parent => new UserCommandResponseModel
            {
                ParentId = parent.ParentId,
                CommandId = parent.CommandId,
                Label = parent.Label,
                CommandType = parent.CommandType,
                Href = parent.CommandUri,
                RoleId = parent.RoleId,
                RoleName = parent.RoleName,
                Invoke = parent.Invoke ? "true" : "false",
                Approve = parent.Approve ? "true" : "false",
                Icon = parent.Icon,
                GroupMenuVisible = parent.GroupMenuVisible,
                Prefix = parent.GroupMenuId,
                GroupMenuListAuthorizeForm = parent.GroupMenuListAuthorizeForm,
                GroupMenuId = parent.GroupMenuId,
                IsAgreement = parent.IsAgreement,
                Children = [.. uniqueMenus
                    .Where(child => child.ParentId == parent.CommandId)
                    .Select(child => new UserCommandResponseModel
                    {
                        ParentId = child.ParentId,
                        CommandId = child.CommandId,
                        Label = child.Label,
                        CommandType = child.CommandType,
                        Href = child.CommandUri,
                        RoleId = child.RoleId,
                        RoleName = child.RoleName,
                        Invoke = child.Invoke ? "true" : "false",
                        Approve = child.Approve ? "true" : "false",
                        Icon = child.Icon,
                        GroupMenuVisible = child.GroupMenuVisible,
                        Prefix = child.GroupMenuId,
                        GroupMenuListAuthorizeForm = child.GroupMenuListAuthorizeForm,
                        GroupMenuId = child.GroupMenuId,
                        IsAgreement = child.IsAgreement,
                        Children = null,
                    })],
            })
            .ToList();

        var userAccount = await _userAccountService.GetByUserCodeAsync(model.CurrentUserCode);

        var avatarRecord = await _userAvatarService.GetByUserCodeAsync(userAccount.UserCode);

        var openApiConfig = Singleton<AppSettings>.Instance.Get<O24OpenAPIConfiguration>();

        string avatarUrl = avatarRecord?.ImageUrl;
        if (string.IsNullOrWhiteSpace(avatarUrl))
        {
            var imagePath = "/uploads/avatars/default.png";
            var baseUrl = openApiConfig.OpenAPICMSURI;
            avatarUrl = $"{baseUrl}/{imagePath}";
        }

        var userAuthen = await _userAuthenService.GetByUserCodeAsync(userAccount.UserCode);
        var userBanner = await _userBannerService.GetUserBannerAsync(userAccount.UserCode);
        ApplicationInfoResponseModel responsedata = new(
            userCode: model.CurrentUserCode,
            fullName: $"{userAccount.FirstName ?? ""} {userAccount.MiddleName ?? ""} {userAccount.LastName ?? ""}".Trim(),
            avatar: avatarUrl,
            userCommand: menuHierarchy,
            loginName: userAccount.LoginName,
            lastLoginTime: userAccount.LastLoginTime,
            contractNumber: userAccount.ContractNumber,
            isBiometricSupported: userAccount.IsBiometricSupported,
            isSmartOTPActive: userAuthen?.IsActive ?? false,
            isLogin: userAccount.IsLogin,
            role: listRoleofUser,
            isFirstLogin: userAccount.IsFirstLogin,
            userBanner: userBanner ?? ""

        );
        return responsedata;
    }

    public async Task<JToken> ChangePasswordByO24User(ChangePasswordO24OpenAPIRequestModel model)
    {
        var userAccount = await _userAccountService.GetLoginAccount(
            model.LoginName,
            password: model.Password,
            model.ChannelId,
            model.Language
        ) ?? throw await O24Exception.CreateAsync(
               O24CTHResourceCode.Operation.ChangePasswordError,
               model.Language
           );
        string hashPassword = O9Encrypt.sha_sha256(model.NewPassword, userAccount.UserCode);

        var userPassword = await _userPasswordService.GetByUserCodeAsync(userAccount.UserCode);

        if (userPassword == null)
        {
            throw new O24OpenAPIException($"This user {model.LoginName} have no user password");
        }
        else
        {
            userPassword.Password = hashPassword;
            userPassword.UpdatedOnUtc = DateTime.UtcNow;
            await _userPasswordService.UpdateAsync(userPassword);
            userAccount.IsFirstLogin = false;
            userAccount.UpdatedOnUtc = DateTime.UtcNow;
            userAccount.IsLogin = false;
            await _userAccountService.UpdateAsync(userAccount);
        }

        return JToken.FromObject(new { success = true });
    }

    public async Task<VerifyUserResponseModel> VerifyUserAsync(VerifyUserRequestModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Username))
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                model.Language
            );
        }

        if (string.IsNullOrWhiteSpace(model.Email))
        {
            model.Email = "";
        }

        try
        {
            var userAccount = await _userAccountService.GetByLoginNameAndEmailAsync(
                model.Username,
                model.Email,
                model.PhoneNumber
            );
            return userAccount == null
                ? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language
                )
                : new VerifyUserResponseModel
                {
                    IsVerified = true,
                    ContractNumber = userAccount.ContractNumber,
                    UserCode = userAccount.UserCode,
                };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            Console.WriteLine($"VerifyUser=Exception={ex.Message}\nStackTrace={ex.StackTrace}");
            throw;
        }
    }

    public async Task<bool> RegisterUserAuthenAsync(RegisterUserAuthenModel model)
    {
        try
        {
            var exists = await _userAuthenService.GetByUserCodeAsync(model.UserCode);

            if (exists != null && exists.IsActive == true)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.SmartOTPExisting,
                    model.Language,
                    [model.UserCode]
                );
            }
            else
            {
                string keyString = Guid.NewGuid().ToString();
                string otpCodeString = model.SmartOTPCode;
                string encryptedSmartOTP = OtpCryptoUtil.EncryptSmartOTP(keyString, otpCodeString);
                if (exists != null && exists.IsActive == false)
                {
                    var userAuthen = exists;
                    userAuthen.Key = keyString;
                    userAuthen.SmartOTP = encryptedSmartOTP;
                    userAuthen.UpdatedOnUtc = DateTime.UtcNow;
                    userAuthen.IsActive = true;
                    await _userAuthenService.UpdateAsync(userAuthen);
                }
                else
                {
                    var userAuthen = new UserAuthen
                    {
                        ChannelId = ChannelId.MobileBanking,
                        AuthenType = model.AuthenType,
                        UserCode = model.UserCode,
                        Phone = model.PhoneNumber,
                        Key = keyString,
                        SmartOTP = encryptedSmartOTP,
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow,
                        IsActive = true,
                    };

                    await _userAuthenService.AddAsync(userAuthen);
                }
            }

            return true;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(ex, "Failed to register SmartOTP");
            return false;
        }
    }

    public async Task<VerifySmartOTPResponseModel> VerifySmartOTPCodeAsync(
        VerifyUserAuthenModel model
    )
    {
        var userAuthen =
            await _userAuthenService.GetByUserAuthenInfoAsync(
                model.UserCode,
                model.AuthenType,
                model.PhoneNumber
            )
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPIncorrect,
                model.Language
            );

        var otpCodeString = model.SmartOTPCode;

        var (_, decryptedOtp) =
            OtpCryptoUtil.DecryptSmartOTP(userAuthen.Key, userAuthen.SmartOTP)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                model.Language
            );

        if (otpCodeString != decryptedOtp)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPIncorrect,
                model.Language
            );
        }

        return new VerifySmartOTPResponseModel { IsValid = true, StoredSecretKey = userAuthen.Key };
    }

    public async Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequest model)
    {
        var userSessions =
            await _userSessionService.GetByRefreshToken(model.RefreshToken)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.InvalidSessionRefresh,
                model.Language,
                model.CurrentUserCode
            );

        var loginName = userSessions.LoginName;

        var validateSessionModel = await _userSessionService.CheckValidSingleSession(
            loginName,
            model.Language
        );
        if (!validateSessionModel.IsValid)
        {
            throw await O24Exception.CreateWithNextActionAsync(
                O24CTHResourceCode.Operation.InvalidSessionStatus,
                $"LOGIN",
                model.Language,
                [loginName]
            );
        }

        var userAccount = await _userAccountService.GetByLoginNameAsync(loginName);
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(_setting.TokenLifetimeDays));

        if (userAccount.Status != Common.ACTIVE)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.AccountStatusInvalid,
                model.Language,
                [loginName]
            );
        }

        var token = _jwtTokenService.GetNewJwtToken(
            new User
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

        var refreshToken = JwtTokenService.GenerateRefreshToken();

        await _userSessionService.RevokeByLoginName(loginName);

        var userSession = new UserSession
        {
            Token = token.Hash(),
            UserId = userAccount.UserId,
            LoginName = loginName,
            Reference = userSessions.Reference,
            IpAddress = userSessions.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = model.ChannelId,
            RefreshToken = refreshToken.Hash(),
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = userSessions.ChannelRoles,
            UserCode = userAccount.UserCode,
            BranchCode = userAccount.BranchID,
            UserName = userAccount.UserName,
        };

        await _userSessionService.Insert(userSession);

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = Guid.NewGuid().ToString();
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;

        await _userAccountService.UpdateAsync(userAccount);

        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }

    public async Task<AuthResponseModel> RefreshTokenTeller(RefreshTokenTellerRequest model)
    {
        var userSessions =
            await _userSessionService.GetByRefreshToken(model.OldRefreshToken)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.InvalidSessionRefresh,
                model.Language,
                model.CurrentUserCode
            );

        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(_setting.TokenLifetimeDays));

        var token = model.CoreToken;

        var refreshToken = model.RefreshToken;

        if (string.IsNullOrEmpty(refreshToken))
        {
            refreshToken = JwtTokenService.GenerateRefreshToken();
        }

        await _userSessionService.Revoke(model.Token);

        var userSession = new UserSession
        {
            Token = token.Hash(),
            UserId = userSessions.UserId,
            LoginName = userSessions.LoginName,
            Reference = userSessions.Reference,
            IpAddress = userSessions.IpAddress,
            ExpiresAt = expireTime,
            ChannelId = model.ChannelId,
            RefreshToken = refreshToken.Hash(),
            RefreshTokenExpiresAt = expireTime,
            ChannelRoles = userSessions.ChannelRoles,
            UserCode = userSessions.UserCode,
            BranchCode = userSessions.BranchCode,
            UserName = userSessions.UserName,
        };

        await _userSessionService.Insert(userSession);

        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }

    public async Task<bool> LogoutAsync(LogoutO24OpenAPIRequestModel model)
    {
        try
        {
            var loginName = model.LoginName;
            var channelid = model.ChannelId;

            var userAccount =
                await _userAccountService.GetByLoginNameandChannelAsync(loginName, channelid)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language,
                    [loginName]
                );

            userAccount.IsLogin = false;
            await _userAccountService.UpdateAsync(userAccount);
            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(ResourceCode.Common.ServerError, model.Language);
        }
    }

    public async Task<ResetPasswordResponseModel> ResetPasswordAsync(
        ResetPasswordRequestModel model
    )
    {
        try
        {
            var usercode = model.UserCode;

            var userAccount =
                await _userAccountService.GetByUserCodeAsync(usercode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language,
                    [usercode]
                );

            var userAccountPassword =
                await _userPasswordService.GetByUserCodeAsync(usercode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.PasswordDonotSetting,
                    model.Language,
                    [usercode]
                );

            string newPassword = PasswordUtils.GenerateRandomPassword(10);
            string hashPassword = O9Encrypt.sha_sha256(newPassword, usercode);

            userAccountPassword.Password = hashPassword;
            userAccountPassword.UpdatedOnUtc = DateTime.UtcNow;

            await _userPasswordService.UpdateAsync(userAccountPassword);

            userAccount.IsLogin = false;
            userAccount.Status = Common.ACTIVE;
            userAccount.Failnumber = 0;
            userAccount.IsFirstLogin = true;
            await _userAccountService.UpdateAsync(userAccount);
            await _userSessionService.RevokeByLoginName(userAccount.LoginName);

            if (userAccount.UserCode == model.CurrentUserCode)
            {
                var userDevice = await _userDeviceService.GetByUserCodeAsync(userAccount.UserCode) ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UserDeviceNotExist,
                    model.Language,
                    [userAccount.UserCode]
                );

                var userPublishEvent = new DefaultModel
                {
                    UserCode = userAccount.UserCode,
                    UserName = userAccount.UserName,
                    DeviceId = userDevice.DeviceId ?? model.DeviceId
                };
                await PublishEventUserLogout(userPublishEvent);
            }

            var payload = _notificationBuilder.BuildResetPasswordNotification(
                usercode,
                model.PhoneNumber,
                userAccount,
                newPassword
            );

            return payload;
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.ServerError,
                model.Language,
                [ex.Message]
            );
        }
    }

    public async Task<bool> IsLoginAsync(DefaultModel model)
    {
        var userAccount =
            await _userAccountService.GetByUserCodeAsync(model.UserCode)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                model.Language,
                [model.UserCode]
            );

        return userAccount.IsLogin ?? false;
    }

    public async Task<UserResponseModel> CreateUserAsync(CreateUserRequestModel model)
    {
        var now = DateTime.UtcNow;
        string userId = null;
        string userCode = null;
        bool isUserAccountCreated = false;
        bool isUserPasswordCreated = false;
        bool isUserInRolesCreated = false;

        try
        {
            if (model.IsReverse)
            {
                await _userInRoleService.DeleteByUserCodeAsync(userCode);
                await _userPasswordService.DeletePasswordByUserIdAsync(userId);
                await _userAccountService.DeleteUserByUserIdAsync(userId);
                return new UserResponseModel { };
            }

            // 1. Check username existence
            var isUserExisting = await _userAccountService.IsExist(model.UserName);
            if (isUserExisting)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsExisting,
                    model.Language,
                    [model.UserName]
                );
            }

            // 2. Generate ID and hash password
            userId = Guid.NewGuid().ToString();
            userCode = userId;
            string password = model.Password ?? PasswordUtils.GenerateRandomPassword(8);
            string salt = PasswordUtils.GenerateRandomSalt();
            string hashPassword = O9Encrypt.sha_sha256(password, userCode);

            var userchannel = string.IsNullOrEmpty(model.ContractType)
                ? "BO"
                : (model.ContractType == "IND" ? "MB" : "AM");

            var listofRoleUser = await _userRightService.GetListRoleIdByChannelAsync(userchannel);
            var roleArrayString =
                listofRoleUser != null && listofRoleUser.Count != 0
                    ? $"[{string.Join(",", listofRoleUser)}]"
                    : "[1]";
            // 3. Create UserAccount
            var userAccount = new UserAccount
            {
                ChannelId = userchannel,
                UserId = userId,
                UserName = model.UserName,
                UserCode = userCode,
                LoginName = model.UserName,
                RoleChannel = roleArrayString,
                Status = Common.ACTIVE,
                UserCreated = model.CurrentUserCode,
                IsSuperAdmin = false,
                BranchID = !string.IsNullOrWhiteSpace(model.CurrentBranchCode)
                    ? model.CurrentBranchCode
                    : "0",
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                Gender = model.Gender,
                Address = model.Address,
                Phone = model.Phone,
                Birthday = DateTime.TryParse(model.Birthday, out var birthday) ? birthday : null,
                PolicyID = model.PolicyId,
                UserLevel = int.Parse(model.UserLevel),
                CreatedOnUtc = now,
                IsShow = Constant.Code.ShowStatus.YES,
                ContractNumber = model.ContractNumber,
                IsFirstLogin = true,
                NotificationType = !string.IsNullOrWhiteSpace(model.NotificationType)
                    ? model.NotificationType
                    : "MAIL",
            };
            await _userAccountService.AddAsync(userAccount);
            isUserAccountCreated = true;

            // 4. Create UserPassword
            var userPassword = new UserPassword
            {
                ChannelId = userchannel,
                UserId = userId,
                Password = hashPassword,
                Salt = salt,
                CreatedOnUtc = now,
            };
            await _userPasswordService.AddAsync(userPassword);
            isUserPasswordCreated = true;

            // 5. Create UserInRole
            var listUserRole = await _userRoleService.GetByRoleTypeAsync(model.UserType);

            var userInRoles = listUserRole
                .Select(
                    (roleId, index) =>
                        new UserInRole
                        {
                            UserCode = userCode,
                            IsMain = index == 0 ? "Y" : "N",
                            CreatedOnUtc = now,
                            RoleId = roleId,
                        }
                )
                .ToList();

            if (userInRoles.Count > 0)
            {
                await _userInRoleService.BulkInsert(userInRoles);
                isUserInRolesCreated = true;
            }

            var nameParts = new[] { model.FirstName, model.MiddleName, model.LastName };
            var fullname = string.Join(
                " ",
                nameParts.Where(part => !string.IsNullOrWhiteSpace(part))
            );

            // 6. Build response
            var response = new UserResponseModel
            {
                DataTemplate = new
                {
                    userCode,
                    model.UserName,
                    model.Email,
                    fullname,
                    model.Phone,
                    password,
                },
                MimeEntities = [],
            };

            var qrImageBytes = StringExtensions.GenerateQRCodeBytes(password);
            response.MimeEntities.Add(
                new DTSMimeEntityModel
                {
                    ContentType = "image/png",
                    ContentId = "qr",
                    Base64 = Convert.ToBase64String(qrImageBytes),
                }
            );

            return response;
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();

            try
            {
                if (isUserInRolesCreated)
                {
                    await _userInRoleService.DeleteByUserCodeAsync(userCode);
                }

                if (isUserPasswordCreated)
                {
                    await _userPasswordService.DeletePasswordByUserIdAsync(userId);
                }

                if (isUserAccountCreated)
                {
                    await _userAccountService.DeleteUserByUserIdAsync(userId);
                }
            }
            catch (Exception reverseEx)
            {
                await reverseEx.LogErrorAsync();
            }

            throw await O24Exception.CreateAsync(
                ResourceCode.Common.ServerError,
                model.Language,
                [ex.Message]
            );
        }
    }

    public async Task<bool> ChangeOwnerPasswordAsync(ChangeOwnerRequestModel model)
    {
        var userAccount = await _userAccountService.GetByUserCodeAsync(model.CurrentUserCode)
        ?? throw await O24Exception.CreateAsync(O24CTHResourceCode.Operation.ChangePasswordError, model.Language);

        string hashPassword = O9Encrypt.sha_sha256(model.Password, userAccount.UserCode);

        var userPassword = await _userPasswordService.GetByUserCodeAsync(userAccount.UserCode);

        if (userPassword == null)
        {
            throw new O24OpenAPIException(
                $"This user {userAccount.LoginName} have no user password"
            );
        }
        else
        {
            bool isPasswordValid;
            try
            {
                isPasswordValid = PasswordUtils.VerifyPassword(
                    usercode: userAccount.UserCode,
                    password: model.OldPassword,
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
                throw new O24OpenAPIException("Current password is incorrect");
            }
            userPassword.Password = hashPassword;
            userPassword.UpdatedOnUtc = DateTime.UtcNow;
            await _userPasswordService.UpdateAsync(userPassword);
            userAccount.IsFirstLogin = false;
            userAccount.UpdatedOnUtc = DateTime.UtcNow;
            userAccount.IsLogin = false;
            await _userAccountService.UpdateAsync(userAccount);
        }
        await _userSessionService.RevokeByLoginName(userAccount.LoginName);

        return true;
    }

    public async Task<bool> DeactivateSmartOTPAsync(RegisterUserAuthenModel model)
    {
        var existingAuthen = await _userAuthenService.GetByUserCodeAsync(model.UserCode);

        if (existingAuthen == null || existingAuthen.IsActive != true)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPNotFound,
                model.Language,
                [model.UserCode]
            );
        }
        existingAuthen.UpdatedOnUtc = DateTime.UtcNow;
        existingAuthen.IsActive = false;

        await _userAuthenService.UpdateAsync(existingAuthen);

        return true;
    }

    /// <summary>
    /// Publish User Logout Event
    /// </summary>
    /// <param name="userLogoutEvent"></param>
    /// <returns></returns>
    public static async Task PublishEventUserLogout(DefaultModel userLogoutEvent)
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        var @event = new UserLogoutEvent
        {
            UserCode = userLogoutEvent.UserCode,
            UserName = userLogoutEvent.UserName,
            DeviceId = userLogoutEvent.DeviceId,
        };
        BusinessLogHelper.Info("Publishing UserLogoutEvent: {0};{1};{2}", @event.UserCode, @event.UserName, @event.DeviceId);
        var cancellationToken = new CancellationToken();
        await eventBus.PublishAsync(@event, cancellationToken);
    }
}
