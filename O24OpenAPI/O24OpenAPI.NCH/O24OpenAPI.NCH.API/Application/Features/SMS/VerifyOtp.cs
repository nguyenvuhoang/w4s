using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Constant;
using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;
using O24OpenAPI.NCH.Infrastructure.Repositories;

namespace O24OpenAPI.NCH.API.Application.Features.Sms;

public class VerifyOtpCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string OTP { get; set; }
    public string VerifyOTPCode { get; set; }
}

[CqrsHandler]
public class VerifyOtpHandle(
    IStoreOtpRepository storeOtpRepository,
    IOTPRequestRepository oTPRequestRepository
) : ICommandHandler<VerifyOtpCommand, bool>
{
    public async Task<bool> HandleAsync(
        VerifyOtpCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var encryptedOtp = Utils.Utility.EncryptOTP(request.OTP);

        if (string.IsNullOrEmpty(request.VerifyOTPCode))
        {
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.InvalidOTP,
                request.Language,
                [request.OTP]
            );
        }

        var storeOtp = await storeOtpRepository
            .Table.Where(x =>
                x.IsActive
                && x.PhoneNumber == request.PhoneNumber
                && (int)x.Platform == (int)ReviewPlatform.Any
                && (x.StartAt == null || x.StartAt <= DateTime.UtcNow)
                && (x.EndAt == null || x.EndAt >= DateTime.UtcNow)
            )
            .FirstOrDefaultAsync();

        if (storeOtp != null)
        {
            if (!encryptedOtp.Equals(storeOtp.OtpHash))
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.InvalidOTP,
                    request.Language,
                    [request.OTP]
                );
            }
            return true;
        }

        string userCode = request.UserCode ?? request.CurrentUserCode;

        var otpRecord =
            await oTPRequestRepository
                .Table.Where(o =>
                    o.TransactionId == request.VerifyOTPCode
                    && o.UserCode == userCode
                    && o.PhoneNumber == request.PhoneNumber
                    && o.Purpose == request.Purpose
                    && o.OtpCode == encryptedOtp
                )
                .FirstOrDefaultAsync()
            ?? throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.InvalidOTP,
                request.Language,
                [request.OTP]
            );

        if (otpRecord.IsUsed)
        {
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.UsedOTP,
                request.Language,
                [request.OTP]
            );
        }

        if (otpRecord.ExpiresAt <= DateTime.UtcNow)
        {
            otpRecord.IsUsed = true;
            otpRecord.Status = OTP_REQUESTS_STATUS.EXPIRED;
            await oTPRequestRepository.Update(otpRecord);

            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.ExpiredOTP,
                request.Language,
                [request.OTP]
            );
        }

        otpRecord.IsUsed = true;
        otpRecord.Status = OTP_REQUESTS_STATUS.SUCCESS;
        await oTPRequestRepository.Update(otpRecord);

        return true;
    }
}
