using System.Globalization;
using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Models.PMT;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.Models.PMTModels;
using O24OpenAPI.PMT.API.Application.Helpers;
using O24OpenAPI.PMT.Infrastructure.Configurations;

namespace O24OpenAPI.PMT.API.Application.Features.VNPay;

[CqrsHandler]
public class VNPayProcessPayHandle(IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<VNPayProcessPayCommand, VNPayProcessPayResponseModel>
{
    public async Task<VNPayProcessPayResponseModel> HandleAsync(
        VNPayProcessPayCommand request,
        CancellationToken cancellationToken = default
    )
    {
        _ = cancellationToken;
        decimal amount = decimal.TryParse(
            request.Amount,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out decimal amt
        )
            ? amt
            : 0m;
        if (amount <= 0)
            throw new O24OpenAPIException("Invalid amount.");

        string transactionId = new SnowflakeTransactionNumberGenerator(
            machineId: 1
        ).GenerateTransactionNumber();

        HttpContext httpContext =
            httpContextAccessor.HttpContext
            ?? throw new O24OpenAPIException("HttpContext is not available.");

        VNPayConfiguration cfg = Singleton<AppSettings>.Instance.Get<VNPayConfiguration>();

        string vnp_Url = cfg.BaseUrl; // VNPAY pay endpoint (sandbox/prod)
        string returnUrl = cfg.ReturnUrl; // Your API endpoint (/vnpay/return)
        string vnp_TmnCode = cfg.TmnCode;
        string vnp_HashSecret = cfg.HashSecret;
        string vnp_Version = cfg.Version;

        if (
            string.IsNullOrWhiteSpace(vnp_Url)
            || string.IsNullOrWhiteSpace(returnUrl)
            || string.IsNullOrWhiteSpace(vnp_TmnCode)
            || string.IsNullOrWhiteSpace(vnp_HashSecret)
        )
        {
            throw new O24OpenAPIException(
                "VNPAY config is missing (BaseUrl/ReturnUrl/TmnCode/HashSecret)."
            );
        }

        //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
        long amountX100 = checked((long)Math.Round(amount, 0, MidpointRounding.AwayFromZero) * 100);

        DateTime now = DateTime.Now;
        string createDate =
            request.VNPayTransactionDate
            ?? now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        string ipAddr = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        string language = request.LanguageCode?.Trim().ToLowerInvariant() switch
        {
            "vi" or "vi-vn" => "vn",
            "en" or "en-us" => "en",
            _ => "en",
        };

        //Build URL for VNPAY
        VnPayLibrary vnpay = new();

        vnpay.AddRequestData("vnp_Version", vnp_Version ?? VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", amountX100.ToString());

        vnpay.AddRequestData("vnp_CreateDate", createDate);
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_IpAddr", ipAddr);

        vnpay.AddRequestData("vnp_Locale", language);

        vnpay.AddRequestData(
            "vnp_OrderInfo",
            request.TransactionDescription ?? $"Payment for order: {transactionId}"
        );
        vnpay.AddRequestData("vnp_OrderType", "other");

        vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
        vnpay.AddRequestData("vnp_TxnRef", transactionId);

        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

        return new VNPayProcessPayResponseModel
        {
            TransactionRef = transactionId,
            PaymentUrl = paymentUrl,
            TransactionDate = createDate,
        };
    }
}
