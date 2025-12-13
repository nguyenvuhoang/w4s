using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Data;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Infrastructure;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Request.SMS;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Exceptions;
using O24OpenAPI.Web.Framework.Extensions;
using System.Globalization;
using System.Security.Cryptography;

namespace O24OpenAPI.O24NCH.Services.Services;

public class SMSService(
    IRepository<OTP_REQUESTS> otpRequestRepository,
    IRepository<SMSTemplate> smsTemplateRepository,
    ISMSProviderService smsProviderService,
    SMSSetting smsSettings,
    IRepository<StoreOtp> storeOtpRepository
) : ISMSService
{
    private readonly IRepository<OTP_REQUESTS> _otpRequestRepository = otpRequestRepository;
    private readonly IRepository<SMSTemplate> _smsTemplateRepository = smsTemplateRepository;
    private readonly ISMSProviderService _smsProviderService = smsProviderService;
    private readonly SMSSetting _smsSettings = smsSettings;
    private readonly IRepository<StoreOtp> _storeOtpRepository = storeOtpRepository;


    /// <summary>
    /// Generate và gửi OTP
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<GenerateOTPResponseModel> GenerateAndSendOTPAsync(GenerateOTPRequestModel model)
    {
        try
        {
            var now = DateTime.UtcNow;

            var transactionId = string.IsNullOrWhiteSpace(model.RefId)
                ? Guid.NewGuid().ToString("N")
                : model.RefId;

            var userCode = model.UserCode ?? model.CurrentUserCode;

            var existingOtp = await _otpRequestRepository.Table
                .Where(o =>
                    o.PhoneNumber == model.PhoneNumber &&
                    o.Purpose == model.Purpose &&         // rất quan trọng: lock theo mục đích
                    !o.IsUsed &&
                    o.ExpiresAt > now &&
                    (o.Status == OTP_REQUESTS_STATUS.PENDING ||
                     o.Status == OTP_REQUESTS_STATUS.SENT)) // FAILED không block
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (existingOtp != null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.ExistOTP,
                    model.Language,
                    [userCode]);
            }

            var otpNumber = RandomNumberGenerator.GetInt32(100000, 1_000_000);
            var otp = otpNumber.ToString(CultureInfo.InvariantCulture);

            var encryptedOtp = Utils.Utility.EncryptOTP(otp);

            var otpRequest = new OTP_REQUESTS
            {
                UserCode = userCode,
                PhoneNumber = model.PhoneNumber,
                OtpCode = encryptedOtp,
                Purpose = model.Purpose,
                IsUsed = false,
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(_smsSettings.TimeOTP),
                TransactionId = transactionId,
                Status = OTP_REQUESTS_STATUS.PENDING
            };

            await _otpRequestRepository.InsertAsync(otpRequest);

            var message = await BuildSMSContentAsync(
                model.Purpose,
                new Dictionary<string, string>
                {
                    ["OTP"] = otp,
                    ["EXPIRE"] = _smsSettings.TimeOTP.ToString(CultureInfo.InvariantCulture),
                    ["PHONENUMBER"] = model.PhoneNumber,
                    ["ACCOUNT"] = model.Account,
                    ["AMOUNT"] = model.Amount.HasValue
                        ? model.Amount.Value.ToString("N2", CultureInfo.InvariantCulture)
                        : string.Empty,
                    ["CURRENCY"] = model.Currency ?? string.Empty
                });

            var sendResult = await _smsProviderService.SendSMSAsync(
                model.PhoneNumber,
                message,
                transactionId,
                null,
                transactionId);

            otpRequest.Status = sendResult.IsSuccess
                ? OTP_REQUESTS_STATUS.SENT
                : OTP_REQUESTS_STATUS.FAILED;

            await _otpRequestRepository.Update(otpRequest);

            if (!sendResult.IsSuccess)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Error.SendSMSFailed,
                    model.Language,
                    [sendResult.ErrorMessage]);
            }

            return new GenerateOTPResponseModel
            {
                TransactionId = transactionId
            };
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



    public async Task<string> BuildSMSContentAsync(string templateCode, Dictionary<string, string> values)
    {
        var template = await _smsTemplateRepository.Table
            .FirstOrDefaultAsync(t => t.TemplateCode == templateCode && t.IsActive);

        if (template == null)
        {
            return string.Empty;
        }

        string content = template.MessageContent;

        foreach (var item in values)
        {
            content = content.Replace($"{{{item.Key}}}", item.Value);
        }

        return content;
    }

    public async Task<bool> VerifyOTPAsync(VeriveryOTPRequestModel model)
    {
        var encryptedOtp = Utils.Utility.EncryptOTP(model.OTP);

        if (string.IsNullOrEmpty(model.VerifyOTPCode))
        {
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.InvalidOTP,
                model.Language,
                [model.OTP]);
        }

        var storeOtp = await _storeOtpRepository.Table
        .Where(x => x.IsActive
                    && x.PhoneNumber == model.PhoneNumber
                    && (x.Platform == ReviewPlatform.Any)
                    && (x.StartAt == null || x.StartAt <= DateTime.UtcNow)
                    && (x.EndAt == null || x.EndAt >= DateTime.UtcNow))
        .FirstOrDefaultAsync();

        if (storeOtp != null)
        {
            if (!encryptedOtp.Equals(storeOtp.OtpHash))
            {
                throw await O24Exception.CreateAsync(
                   O24NCHResourceCode.Validation.InvalidOTP,
                   model.Language,
                   [model.OTP]);
            }
            return true;
        }

        string userCode = model.UserCode ?? model.CurrentUserCode;

        var otpRecord = await _otpRequestRepository.Table
         .Where(o =>
             o.TransactionId == model.VerifyOTPCode &&
             o.UserCode == userCode &&
             o.PhoneNumber == model.PhoneNumber &&
             o.Purpose == model.Purpose &&
             o.OtpCode == encryptedOtp)
         .FirstOrDefaultAsync()
         ?? throw await O24Exception.CreateAsync(O24NCHResourceCode.Validation.InvalidOTP, model.Language, [model.OTP]);

        if (otpRecord.IsUsed)
        {
            throw await O24Exception.CreateAsync(O24NCHResourceCode.Validation.UsedOTP, model.Language, [model.OTP]);
        }

        if (otpRecord.ExpiresAt <= DateTime.UtcNow)
        {
            otpRecord.IsUsed = true;
            otpRecord.Status = OTP_REQUESTS_STATUS.EXPIRED;
            await _otpRequestRepository.Update(otpRecord);

            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.ExpiredOTP,
                model.Language,
                [model.OTP]);
        }

        otpRecord.IsUsed = true;
        otpRecord.Status = OTP_REQUESTS_STATUS.SUCCESS;
        await _otpRequestRepository.Update(otpRecord);

        return true;
    }


    public async Task<string> BuildSMSContentDynamicAsync(string templateCode, Dictionary<string, object> values, string message = "")
    {
        var template = await _smsTemplateRepository.Table
            .FirstOrDefaultAsync(t => t.TemplateCode == templateCode && t.IsActive);

        if (template == null)
        {
            return message;
        }

        string content = template.MessageContent;

        foreach (var kvp in values)
        {
            content = content.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString());
        }

        return content;
    }


    public async Task<bool> SendSMS(SMSRequestModel model)
    {
        try
        {
            var message = string.IsNullOrWhiteSpace(model.Message)
            ? await BuildSMSContentDynamicAsync(model.Purpose, model.SenderData)
            : model.Message;

            var transactionId = model.RefId;
            var sendResult = await _smsProviderService.SendSMSAsync(model.PhoneNumber, message, transactionId);
            return sendResult.IsSuccess;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
            return false;
        }
    }

    public Task<RetrieveSMSInfoResponseModel> RetrieveInfoAsync(RetrieveSMSInfoRequestModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<SMSGatewayResponseModel> SMSGatewaySend(SMSGatewayRequestModel model)
    {
        try
        {
            var message = await BuildSMSContentDynamicAsync(model.Purpose, model.SenderData, model.Message);

            // Generate transaction ID
            var transactionId = model.RefId;

            // Call provider service to send SMS
            var submitResult = await _smsProviderService.SendSMSAsync(
                model.PhoneNumber, message, transactionId, model.ProviderName, model.TransactionId, model.MessageType);

            // Map the result to response model
            var sendResult = new SMSGatewayResponseModel
            {
                PhoneNumber = model.PhoneNumber,
                Message = message,
                TransactionId = model.TransactionId,
                Provider = model.ProviderName,
                IsSuccess = submitResult.IsSuccess,
                SMSGWTransactionId = transactionId
            };

            if (!submitResult.IsSuccess)
            {
                sendResult.ErrorMessage = submitResult.ErrorMessage;
                sendResult.ErrorCode = submitResult.ErrorCode;
            }

            return sendResult;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();

            return new SMSGatewayResponseModel
            {
                PhoneNumber = model.PhoneNumber,
                Message = model.Message,
                TransactionId = model.TransactionId,
                Provider = model.ProviderName,
                IsSuccess = false,
                ErrorMessage = ex.Message,
                ErrorCode = ex.HResult,
                SMSGWTransactionId = model.RefId
            };
        }
    }

    public async Task<GenerateSMSContentResponseModel> GenerateAndSendContentAsync(GenerateSMSContentRequestModel model)
    {
        try
        {
            var transactionId = model.RefId;

            var message = await BuildSMSContentAsync(model.Purpose, new Dictionary<string, string>
            {
                { "CONTRACTNUMBER", model.ContractNumber ?? string.Empty },
                { "PHONENUMBER", model.PhoneNumber },
                { "PASSWORD", model.Password }
            });

            var sendResult = await _smsProviderService.SendSMSAsync(model.PhoneNumber, message, transactionId, null, transactionId);

            return sendResult.IsSuccess ? new GenerateSMSContentResponseModel { TransactionId = transactionId } : null;
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

    /// <summary>
    /// Bulk send SMS cho danh sách yêu cầu
    /// </summary>
    public async Task<bool> BulkSendSMS(List<SMSRequestModel> requests, int maxDegreeOfParallelism = 20, CancellationToken ct = default)
    {
        if (requests == null || requests.Count == 0)
        {
            return true;
        }

        try
        {
            var buildTasks = requests.Select(async r =>
            {
                var msg = string.IsNullOrWhiteSpace(r.Message)
                    ? await BuildSMSContentDynamicAsync(r.Purpose, r.SenderData)
                    : r.Message;

                string providerName = "UNITEL";
                var providerMain = await _smsProviderService.GetProviderByPhoneNumber(r.PhoneNumber);
                if (providerMain != null)
                {
                    providerName = providerMain.ProviderName;
                }
                var uuid = Guid.NewGuid().ToString();
                return (
                    ProviderName: providerName,
                    r.PhoneNumber,
                    Message: msg,
                    TransactionId: uuid,
                    EndToEnd: uuid
                );
            });

            var bulkInputs = (await Task.WhenAll(buildTasks)).ToList();

            var results = await _smsProviderService.SendBulkSMSAsync(
                bulkInputs,
                maxDegreeOfParallelism,
                ct
            );

            var allOk = results.Count > 0 && results.All(x => x.IsSuccess);
            return allOk;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
            return false;
        }
    }


}
