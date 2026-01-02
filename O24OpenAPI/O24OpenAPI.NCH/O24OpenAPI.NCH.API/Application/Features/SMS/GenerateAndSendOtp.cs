using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Constants;
using O24OpenAPI.NCH.API.Application.Features.SMS.Services;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Infrastructure.Configurations;
using System.Globalization;
using System.Security.Cryptography;

namespace O24OpenAPI.NCH.API.Application.Features.Sms;

public class GenerateAndSendOtpCommand : BaseTransactionModel, ICommand<GenerateOTPResponseModel>
{
    public string UserCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string Account { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}

[CqrsHandler]
public class GenerateAndSendOtpHandle(
    IOTPRequestRepository otpRequestRepository,
    ISMSTemplateRepository sMSTemplateRepository,
    SMSSetting sMSSetting,
    ISMSProviderService smsProviderService
) : ICommandHandler<GenerateAndSendOtpCommand, GenerateOTPResponseModel>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_SMS_GENERATE_OTP)]
    public async Task<GenerateOTPResponseModel> HandleAsync(
        GenerateAndSendOtpCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            DateTime now = DateTime.UtcNow;

            string transactionId = string.IsNullOrWhiteSpace(request.RefId)
                ? Guid.NewGuid().ToString("N")
                : request.RefId;

            string userCode = request.UserCode ?? request.CurrentUserCode;

            OTP_REQUESTS existingOtp = await otpRequestRepository
                .Table.Where(o =>
                    o.PhoneNumber == request.PhoneNumber
                    && o.Purpose == request.Purpose
                    && // rất quan trọng: lock theo mục đích
                    !o.IsUsed
                    && o.ExpiresAt > now
                    && (
                        o.Status == OTP_REQUESTS_STATUS.PENDING
                        || o.Status == OTP_REQUESTS_STATUS.SENT
                    )
                ) // FAILED không block
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (existingOtp != null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.ExistOTP,
                    request.Language,
                    [userCode]
                );
            }

            int otpNumber = RandomNumberGenerator.GetInt32(100000, 1_000_000);
            string otp = otpNumber.ToString(CultureInfo.InvariantCulture);

            string encryptedOtp = Utils.Utility.EncryptOTP(otp);

            OTP_REQUESTS otpRequest = new()
            {
                UserCode = userCode,
                PhoneNumber = request.PhoneNumber,
                OtpCode = encryptedOtp,
                Purpose = request.Purpose,
                IsUsed = false,
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(sMSSetting.TimeOTP),
                TransactionId = transactionId,
                Status = OTP_REQUESTS_STATUS.PENDING,
            };

            await otpRequestRepository.InsertAsync(otpRequest);

            string message = await BuildSMSContentAsync(
                request.Purpose,
                new Dictionary<string, string>
                {
                    ["OTP"] = otp,
                    ["EXPIRE"] = sMSSetting.TimeOTP.ToString(CultureInfo.InvariantCulture),
                    ["PHONENUMBER"] = request.PhoneNumber,
                    ["ACCOUNT"] = request.Account,
                    ["AMOUNT"] = request.Amount.HasValue
                        ? request.Amount.Value.ToString("N2", CultureInfo.InvariantCulture)
                        : string.Empty,
                    ["CURRENCY"] = request.Currency ?? string.Empty,
                }
            );

            SendSOAPResponseModel sendResult = await smsProviderService.SendSMSAsync(
                request.PhoneNumber,
                message,
                transactionId,
                null,
                transactionId
            );

            otpRequest.Status = sendResult.IsSuccess
                ? OTP_REQUESTS_STATUS.SENT
                : OTP_REQUESTS_STATUS.FAILED;

            await otpRequestRepository.Update(otpRequest);

            if (!sendResult.IsSuccess)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Error.SendSMSFailed,
                    request.Language,
                    [sendResult.ErrorMessage]
                );
            }

            return new GenerateOTPResponseModel { TransactionId = transactionId };
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync(ex, "Failed to generate or send OTP");
            return null;
        }
    }

    private async Task<string> BuildSMSContentAsync(
        string templateCode,
        Dictionary<string, string> values
    )
    {
        SMSTemplate template = await sMSTemplateRepository.Table.FirstOrDefaultAsync(t =>
            t.TemplateCode == templateCode && t.IsActive
        );

        if (template == null)
        {
            return string.Empty;
        }

        string content = template.MessageContent;

        foreach (KeyValuePair<string, string> item in values)
        {
            content = content.Replace($"{{{item.Key}}}", item.Value);
        }

        return content;
    }
}
