using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils.O9;
using O24OpenAPI.Logging.Helpers;
using QRCoder;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class ResetPasswordCommand : BaseTransactionModel, ICommand<ResetPasswordResponseModel>
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class ResetPasswordHandle(
    IUserAccountRepository userAccountRepository,
    IUserPasswordRepository userPasswordRepository,
    IUserSessionRepository userSessionRepository,
    IStaticCacheManager staticCacheManager,
    IUserDeviceRepository userDeviceRepository
) : ICommandHandler<ResetPasswordCommand, ResetPasswordResponseModel>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_RESET_PASSWORD)]
    public async Task<ResetPasswordResponseModel> HandleAsync(
        ResetPasswordCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<ResetPasswordRequestModel>();
        return await ResetPasswordAsync(model);
    }

    public async Task<ResetPasswordResponseModel> ResetPasswordAsync(
        ResetPasswordRequestModel model
    )
    {
        try
        {
            var usercode = model.UserCode;

            var userAccount =
                await userAccountRepository.GetByUserCodeAsync(usercode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language,
                    [usercode]
                );

            var userAccountPassword =
                await userPasswordRepository.GetByUserCodeAsync(usercode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.PasswordDonotSetting,
                    model.Language,
                    [usercode]
                );

            string newPassword = PasswordUtils.GenerateRandomPassword(10);
            string hashPassword = O9Encrypt.sha_sha256(newPassword, usercode);

            userAccountPassword.Password = hashPassword;
            userAccountPassword.UpdatedOnUtc = DateTime.UtcNow;

            await userPasswordRepository.UpdateAsync(userAccountPassword);

            userAccount.IsLogin = false;
            userAccount.Status = Common.ACTIVE;
            userAccount.Failnumber = 0;
            userAccount.IsFirstLogin = true;

            if (userAccount.UserCode == model.CurrentUserCode)
            {
                var userDevice =
                    await userDeviceRepository.GetByUserCodeAsync(userAccount.UserCode)
                    ?? throw await O24Exception.CreateAsync(
                        O24CTHResourceCode.Validation.UserDeviceNotExist,
                        model.Language,
                        [userAccount.UserCode]
                    );

                var userPublishEvent = new DefaultModel
                {
                    UserCode = userAccount.UserCode,
                    UserName = userAccount.UserName,
                    DeviceId = userDevice.DeviceId ?? model.DeviceId,
                };
                await PublishEventUserLogout(userPublishEvent);
            }
            await userAccountRepository.UpdateAsync(userAccount);
            await RevokeByLoginName(userAccount.LoginName);

            var payload = BuildResetPasswordNotification(
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

    public static async Task PublishEventUserLogout(DefaultModel userLogoutEvent)
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        var @event = new UserLogoutEvent
        {
            UserCode = userLogoutEvent.UserCode,
            UserName = userLogoutEvent.UserName,
            DeviceId = userLogoutEvent.DeviceId,
        };
        BusinessLogHelper.Info(
            "Publishing UserLogoutEvent: {0};{1};{2}",
            @event.UserCode,
            @event.UserName,
            @event.DeviceId
        );
        var cancellationToken = new CancellationToken();
        await eventBus.PublishAsync(@event, cancellationToken);
    }

    public ResetPasswordResponseModel BuildResetPasswordNotification(
        string usercode,
        string phoneNumber,
        UserAccount userAccount,
        string newPassword
    )
    {
        var smsData = new Dictionary<string, object> { { "0", newPassword } };

        var nameParts = new[]
        {
            userAccount.FirstName,
            userAccount.MiddleName,
            userAccount.LastName,
        };
        var fullname = string.Join(" ", nameParts.Where(part => !string.IsNullOrWhiteSpace(part)));

        var qrImageBytes = GenerateQRCodeBytes(newPassword);
        var mimeEntities = new List<DTSMimeEntityModel>
        {
            new()
            {
                ContentType = "image/png",
                ContentId = "qr",
                Base64 = Convert.ToBase64String(qrImageBytes),
            },
        };
        var emailDataTemplate = new Dictionary<string, object>
        {
            { "usercode", usercode },
            { "fullname", fullname },
            { "user_name", userAccount.UserName },
            { "phone", userAccount.Phone },
            { "email", userAccount.Email },
        };

        return new ResetPasswordResponseModel
        {
            UserCode = usercode,
            PhoneNumber = phoneNumber,
            TemplateCode = "RESETPASSWORD",
            NotificationType = userAccount.NotificationType,
            Email = userAccount.Email,
            SmsData = smsData,
            EmailDataTemplate = emailDataTemplate,
            MimeEntities = mimeEntities,
        };
    }

    public static byte[] GenerateQRCodeBytes(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        }

        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);
        return qrCode.GetGraphic(20);
    }
}
