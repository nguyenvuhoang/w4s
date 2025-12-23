using LinKit.Core.Cqrs;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class VerifySmartOTPCodeAsyncCommand
    : BaseTransactionModel,
        ICommand<VerifySmartOTPResponseModel>
{
    public string AuthenType { get; set; }
    public string PhoneNumber { get; set; }
    public string UserCode { get; set; }
    public string SmartOTPCode { get; set; }
}
[CqrsHandler]
public class VerifySmartOTPCodeAsyncHandle(IUserAuthenRepository userAuthenRepository) : ICommandHandler<VerifySmartOTPCodeAsyncCommand, VerifySmartOTPResponseModel>
{
    [WorkflowStep("WF_STEP_CTH_VERIFY_OTP")]
    public async Task<VerifySmartOTPResponseModel> HandleAsync(VerifySmartOTPCodeAsyncCommand request, CancellationToken cancellationToken = default)
    {
        var userAuthen =
       await userAuthenRepository.GetByUserAuthenInfoAsync(
           request.UserCode,
           request.AuthenType,
           request.PhoneNumber
       )
       ?? throw await O24Exception.CreateAsync(
           O24CTHResourceCode.Operation.SmartOTPIncorrect,
           request.Language
       );

        var otpCodeString = request.SmartOTPCode;

        var (_, decryptedOtp) =
            OtpCryptoUtil.DecryptSmartOTP(userAuthen.Key, userAuthen.SmartOTP)
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.UnableDecryptSmartOTP,
                request.Language
            );

        if (otpCodeString != decryptedOtp)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPIncorrect,
                request.Language
            );
        }

        return new VerifySmartOTPResponseModel { IsValid = true, StoredSecretKey = userAuthen.Key };
    }
}
