using LinKit.Core.Cqrs;
using O24OpenAPI.CMS.API.Application.Helpers;
using O24OpenAPI.CMS.Domain.AggregateModels.VNPayAggregate;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.CMS.API.Application.Features.VNPay;

public class VNPayProcessIPNCommand
    : BaseTransactionModel,
        ICommand<VNPayProcessIPNResponseModel>
{
    public Dictionary<string, string> Query { get; set; } = [];
}

public class VNPayProcessIPNResponseModel : BaseO24OpenAPIModel
{
    public string TransactionStatus { get; set; }
    public string TransactionRef { get; set; } = default!;
    public string TransactionStatusMessage { get; set; } = default!;
    public string ResponseCodeStatus { get; set; } = default!;
    public string ResponseCodeMessage { get; set; } = default!;
    public string TransactionDate { get; set; } = default!;
}

[CqrsHandler]
public class VNPayProcessIPNHandler(
    IVNPayResponseCodeMapRepository vNPayResponseCodeMapRepository,
    IVNPayTransactionStatusMapRepository vNPayTransactionStatusMapRepository
)
: ICommandHandler<VNPayProcessIPNCommand, VNPayProcessIPNResponseModel>
{
    public async Task<VNPayProcessIPNResponseModel> HandleAsync(
        VNPayProcessIPNCommand request,
        CancellationToken cancellationToken = default
    )

    {
        var cfg = Singleton<AppSettings>.Instance.Get<VNPayConfiguration>();

        VnPayLibrary vnpay = new();
        foreach (var kv in request.Query)
        {
            if (!kv.Key.StartsWith("vnp_", StringComparison.OrdinalIgnoreCase)) continue;
            if (string.IsNullOrWhiteSpace(kv.Value)) continue;

            if (string.Equals(kv.Key, "vnp_SecureHash", StringComparison.OrdinalIgnoreCase)) continue;

            vnpay.AddResponseData(kv.Key, kv.Value);
        }

        var secureHash = request.Query.GetValueOrDefault("vnp_SecureHash");
        if (string.IsNullOrWhiteSpace(secureHash))
            throw new O24OpenAPIException("MISSING_VNPAY_SECURE_HASH");

        if (!vnpay.ValidateSignature(secureHash, cfg.HashSecret))
            throw new O24OpenAPIException("INVALID_VNPAY_SIGNATURE");

        var responseCode = request.Query.GetValueOrDefault("vnp_ResponseCode")?.Trim();

        var responseCodeTask = await vNPayResponseCodeMapRepository.GetByResponseCodeAsync(
                responseCode,
                cancellationToken
            );

        var transactionStatus = request.Query.GetValueOrDefault("vnp_TransactionStatus")?.Trim();
        var txStatusTask = await vNPayTransactionStatusMapRepository.GetByStatusCodeAsync(
                transactionStatus,
                cancellationToken
            );

        var responseCodeMap = responseCodeTask;
        var txStatusMap = txStatusTask;
        var transactionRef = request.Query.GetValueOrDefault("vnp_TxnRef")?.Trim();
        var transactionDate = request.Query.GetValueOrDefault("vnp_PayDate")?.Trim();

        return new VNPayProcessIPNResponseModel
        {
            TransactionRef = transactionRef,
            TransactionStatus = transactionStatus,
            TransactionStatusMessage = txStatusMap ?? "Không tìm thấy mô tả TransactionStatus trong DB",
            ResponseCodeStatus = responseCode,
            ResponseCodeMessage = responseCodeMap ?? "Không tìm thấy mô tả ResponseCode trong DB",
            TransactionDate = transactionDate
        };
    }
}

