using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Auth;

public class VerifySmartOTPCodeCommand : BaseTransactionModel, ICommand<VerifySmartOTPResponseModel>
{
    public string AuthenType { get; set; }
    public string PhoneNumber { get; set; }
    public string UserCode { get; set; }
    public string SmartOTPCode { get; set; }
}

[CqrsHandler]
public class VerifySmartOTPCodeHandler(IUserAuthenRepository userAuthenRepository)
    : ICommandHandler<VerifySmartOTPCodeCommand, VerifySmartOTPResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_VERIFY_SMARTOTP_PINCODE)]
    public async Task<VerifySmartOTPResponseModel> HandleAsync(
        VerifySmartOTPCodeCommand request,
        CancellationToken cancellationToken = default
    )
    {
        UserAuthen userAuthen =
            await userAuthenRepository.GetByUserAuthenInfoAsync(
                request.UserCode,
                request.AuthenType,
                request.PhoneNumber
            )
            ?? throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Operation.SmartOTPIncorrect,
                request.Language
            );

        string otpCodeString = request.SmartOTPCode;

        (string _, string decryptedOtp) =
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
