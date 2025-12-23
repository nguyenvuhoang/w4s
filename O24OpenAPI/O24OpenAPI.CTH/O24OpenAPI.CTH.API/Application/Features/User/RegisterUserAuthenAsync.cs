using LinKit.Core.Cqrs;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Constants;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class RegisterUserAuthenAsyncCommand : BaseTransactionModel, ICommand<bool>
{
    public string AuthenType { get; set; }
    public string PhoneNumber { get; set; }
    public string UserCode { get; set; }
    public string SmartOTPCode { get; set; }
    public bool IsValidOTP { get; set; }
}

[CqrsHandler]
public class RegisterUserAuthenAsyncHandle(IUserAuthenRepository userAuthenRepository)
    : ICommandHandler<RegisterUserAuthenAsyncCommand, bool>
{
    [WorkflowStep("WF_STEP_CTH_REGISTER_USER")]
    public async Task<bool> HandleAsync(
        RegisterUserAuthenAsyncCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var exists = await userAuthenRepository.GetByUserCodeAsync(request.UserCode);

            if (exists != null && exists.IsActive == true)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Operation.SmartOTPExisting,
                    request.Language,
                    [request.UserCode]
                );
            }
            else
            {
                string keyString = Guid.NewGuid().ToString();
                string otpCodeString = request.SmartOTPCode;
                string encryptedSmartOTP = OtpCryptoUtil.EncryptSmartOTP(
                    keyString,
                    otpCodeString
                );
                if (exists != null && exists.IsActive == false)
                {
                    var userAuthen = exists;
                    userAuthen.Key = keyString;
                    userAuthen.SmartOTP = encryptedSmartOTP;
                    userAuthen.UpdatedOnUtc = DateTime.UtcNow;
                    userAuthen.IsActive = true;
                    await userAuthenRepository.UpdateAsync(userAuthen);
                }
                else
                {
                    var userAuthen = new UserAuthen
                    {
                        ChannelId = ChannelId.MobileBanking,
                        AuthenType = request.AuthenType,
                        UserCode = request.UserCode,
                        Phone = request.PhoneNumber,
                        Key = keyString,
                        SmartOTP = encryptedSmartOTP,
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow,
                        IsActive = true,
                    };

                    await userAuthenRepository.AddAsync(userAuthen);
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
}
