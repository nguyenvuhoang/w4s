using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.EXT.API.Application.Helpers;
using O24OpenAPI.EXT.Infrastructure.Configurations;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using System.Globalization;

namespace O24OpenAPI.EXT.API.Application.Features.VNPay;

public class VNPayProcessPayCommand
    : BaseTransactionModel,
        ICommand<VNPayProcessPayResponseModel>
{
    public decimal Amount { get; set; }
    public string TransactionRef { get; set; }

    public string? OrderInfo { get; set; }
    public string? OrderType { get; set; } = "other";
    public string? BankCode { get; set; }
    public int? ExpireMinutes { get; set; } = 15;

}

public class VNPayProcessPayResponseModel : BaseO24OpenAPIModel
{
    public string PaymentUrl { get; set; } = default!;
    public string TransactionRef { get; set; } = default!;
}

[CqrsHandler]
public class VNPayProcessPayHandle(IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<VNPayProcessPayCommand, VNPayProcessPayResponseModel>
{
    [WorkflowStep(WorkflowStepCode.EXT.WF_STEP_EXT_VNPAY_PROCESS)]
    public async Task<VNPayProcessPayResponseModel> HandleAsync(
        VNPayProcessPayCommand request,
        CancellationToken cancellationToken = default
    )
    {
        _ = cancellationToken;

        if (request.Amount <= 0)
            throw new O24OpenAPIException("Invalid amount.");

        if (string.IsNullOrWhiteSpace(request.TransactionRef))
            throw new O24OpenAPIException("TransactionRef is required.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new O24OpenAPIException("HttpContext is not available.");

        var cfg = Singleton<AppSettings>.Instance.Get<VNPayConfig>();

        var baseUrl = cfg.BaseUrl;        // VNPAY pay endpoint (sandbox/prod)
        var returnUrl = cfg.ReturnUrl;    // Your API endpoint (/vnpay/return)
        var tmnCode = cfg.TmnCode;
        var hashSecret = cfg.HashSecret;

        if (string.IsNullOrWhiteSpace(baseUrl) ||
            string.IsNullOrWhiteSpace(returnUrl) ||
            string.IsNullOrWhiteSpace(tmnCode) ||
            string.IsNullOrWhiteSpace(hashSecret))
        {
            throw new O24OpenAPIException("VNPAY config is missing (BaseUrl/ReturnUrl/TmnCode/HashSecret).");
        }

        var amountX100 = checked((long)Math.Round(request.Amount, 0, MidpointRounding.AwayFromZero) * 100);

        var now = DateTime.Now;
        var createDate = now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        var ipAddr = httpContext.GetClientIPAddress();

        var p = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["vnp_Version"] = cfg.Version ?? "2.1.0",
            ["vnp_Command"] = cfg.Command ?? "pay",
            ["vnp_TmnCode"] = tmnCode,

            ["vnp_Amount"] = amountX100.ToString(CultureInfo.InvariantCulture),
            ["vnp_CurrCode"] = string.IsNullOrWhiteSpace(cfg.CurrencyCode) ? "VND" : cfg.CurrencyCode,

            ["vnp_TxnRef"] = request.TransactionRef.Trim(),
            ["vnp_OrderInfo"] = string.IsNullOrWhiteSpace(request.OrderInfo)
                ? $"Thanh toan giao dich: {request.TransactionRef.Trim()}"
                : request.OrderInfo.Trim(),
            ["vnp_OrderType"] = string.IsNullOrWhiteSpace(request.OrderType) ? "other" : request.OrderType.Trim(),

            ["vnp_Locale"] = string.IsNullOrWhiteSpace(cfg.Locale) ? "vn" : cfg.Locale,
            ["vnp_ReturnUrl"] = returnUrl,
            ["vnp_IpAddr"] = string.IsNullOrWhiteSpace(ipAddr) ? "127.0.0.1" : ipAddr,

            ["vnp_CreateDate"] = createDate,
        };

        if (!string.IsNullOrWhiteSpace(request.BankCode))
            p["vnp_BankCode"] = request.BankCode.Trim();

        if (request.ExpireMinutes is > 0)
        {
            var exp = now.AddMinutes(request.ExpireMinutes.Value)
                         .ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            p["vnp_ExpireDate"] = exp;
        }

        var paymentUrl = VNPaySignature.CreateRequestUrl(baseUrl, p, hashSecret);

        return new VNPayProcessPayResponseModel
        {
            TransactionRef = request.TransactionRef.Trim(),
            PaymentUrl = paymentUrl
        };
    }
}
