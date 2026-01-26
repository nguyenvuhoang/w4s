using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class ChangeDeviceCommand : BaseTransactionModel, ICommand<AuthResponseModel>
{
    public string UserCode { get; set; }
    public string Phone { get; set; }
    public DateTime DOB { get; set; }
    public string LicenseType { get; set; }
    public string LicenseId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Modelname { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string PushId { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public bool IsEmulator { get; set; } = false;
    public bool IsRootedOrJailbroken { get; set; } = false;
}

[CqrsHandler]
public class ChangeDeviceHandle(
    IUserAccountRepository userAccountRepository,
    IUserDeviceRepository userDeviceRepository,
    WebApiSettings webApiSettings,
    IJwtTokenService jwtTokenService,
    IUserRightChannelRepository userRightChannelRepository,
    IUserSessionRepository userSessionRepository
) : ICommandHandler<ChangeDeviceCommand, AuthResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_CHANGE_DEVICE)]
    public async Task<AuthResponseModel> HandleAsync(
        ChangeDeviceCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var userAccount =
            await userAccountRepository.GetByUserCodeAsync(request.UserCode)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsExisting,
                request.Language
            );

        if (userAccount.Phone != request.Phone)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                request.Language,
                [request.Phone]
            );
        }

        if (string.IsNullOrEmpty(userAccount.ContractNumber))
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                request.Language,
                [userAccount.Phone]
            );
        }

        try
        {
            await userDeviceRepository.EnsureUserDeviceAsync(
                userCode: userAccount.UserCode,
                loginName: userAccount.LoginName,
                deviceId: request.DeviceId + request.Modelname ?? "",
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
                isResetDevice: true
            );
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.ChangeDeviceError,
                request.Language
            );
        }

        var context = new LoginContextModel
        {
            DeviceId = request.DeviceId,
            Modelname = request.Modelname,
            RoleChannel = userAccount.RoleChannel,
            IpAddress = request.IpAddress,
            ChannelId = request.ChannelId,
            Reference = request.DeviceId + request.Modelname ?? "",
        };

        userAccount.LastLoginTime = DateTime.Now;
        userAccount.UUID = Guid.NewGuid().ToString();
        userAccount.Failnumber = 0;
        userAccount.IsLogin = true;

        await userAccountRepository.UpdateAsync(userAccount);

        return await CreateTokenAndSessionAsync(userAccount, context);
    }

    public async Task<AuthResponseModel> CreateTokenAndSessionAsync(
        UserAccount userAccount,
        LoginContextModel context
    )
    {
        var currentTime = DateTime.UtcNow;
        var expireTime = currentTime.AddDays(Convert.ToDouble(webApiSettings.TokenLifetimeDays));

        var token = jwtTokenService.GetNewJwtToken(
            new O24OpenAPI.Core.Domain.Users.User
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
        var channelRoles = await userRightChannelRepository.GetSetChannelInRoleAsync(listRoles);

        await userSessionRepository.RevokeByLoginName(userAccount.LoginName);

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

        await userSessionRepository.AddAsync(session);
        return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
    }
}
