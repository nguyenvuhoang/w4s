using LinKit.Core.Cqrs;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Repositories;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class ChangeDeviceCommand : BaseTransactionModel, ICommand<AuthResponseModel>
    {
        public UserAccountChangeDeviceModel Model { get; set; } = default!;
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
        [WorkflowStep("WF_STEP_CTH_CHANGE_DEVICE")]
        public async Task<AuthResponseModel> HandleAsync(
            ChangeDeviceCommand request,
            CancellationToken cancellationToken = default
        )
        {
            return await ChangeDeviceAsync(request.Model);
        }

        public async Task<AuthResponseModel> ChangeDeviceAsync(UserAccountChangeDeviceModel model)
        {
            var userAccount =
                await userAccountRepository.GetByUserCodeAsync(model.UserCode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsExisting,
                    model.Language
                );

            if (userAccount.Phone != model.Phone)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.PhoneNumberIsNotExisting,
                    model.Language,
                    [model.Phone]
                );
            }

            if (string.IsNullOrEmpty(userAccount.ContractNumber))
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.ContractNumberIsNotExisting,
                    model.Language,
                    [userAccount.Phone]
                );
            }

            try
            {
                await userDeviceRepository.EnsureUserDeviceAsync(
                    userCode: userAccount.UserCode,
                    loginName: userAccount.LoginName,
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
                    isResetDevice: true
                );
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync();
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.ChangeDeviceError,
                    model.Language
                );
            }

            var context = new LoginContextModel
            {
                DeviceId = model.DeviceId,
                Modelname = model.Modelname,
                RoleChannel = userAccount.RoleChannel,
                IpAddress = model.IpAddress,
                ChannelId = model.ChannelId,
                Reference = model.DeviceId + model.Modelname ?? "",
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
            var expireTime = currentTime.AddDays(
                Convert.ToDouble(webApiSettings.TokenLifetimeDays)
            );

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

            await userSessionRepository.Insert(session);
            return new AuthResponseModel { Token = token, RefreshToken = refreshToken };
        }
    }
}
