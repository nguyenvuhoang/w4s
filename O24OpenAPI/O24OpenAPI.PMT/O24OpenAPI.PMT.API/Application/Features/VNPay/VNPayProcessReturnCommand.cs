using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.WebUtilities;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.GrpcContracts.Models.PMTModels;
using O24OpenAPI.PMT.API.Application.Helpers;
using O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;
using O24OpenAPI.PMT.Infrastructure.Configurations;

namespace O24OpenAPI.PMT.API.Application.Features.VNPay;

[CqrsHandler]
public class VNPayProcessReturnHandler(
    IVNPayResponseCodeMapRepository vNPayResponseCodeMapRepository,
    IVNPayTransactionStatusMapRepository vNPayTransactionStatusMapRepository
) : ICommandHandler<VNPayProcessReturnCommand, VNPayProcessReturnResponseModel>
{
    public async Task<VNPayProcessReturnResponseModel> HandleAsync(
        VNPayProcessReturnCommand request,
        CancellationToken cancellationToken = default
    )
    {
        VNPayConfiguration cfg = Singleton<AppSettings>.Instance.Get<VNPayConfiguration>();
        Dictionary<string, string> vnpParams = QueryHelpers
            .ParseQuery(request.RawQuery)
            .ToDictionary(x => x.Key, x => x.Value.ToString());
        VnPayLibrary vnpay = new();
        foreach (KeyValuePair<string, string> kv in vnpParams)
        {
            if (!kv.Key.StartsWith("vnp_", StringComparison.OrdinalIgnoreCase))
                continue;
            if (string.IsNullOrWhiteSpace(kv.Value))
                continue;

            if (string.Equals(kv.Key, "vnp_SecureHash", StringComparison.OrdinalIgnoreCase))
                continue;

            vnpay.AddResponseData(kv.Key, kv.Value);
        }

        string secureHash = vnpParams.GetValueOrDefault("vnp_SecureHash");
        if (string.IsNullOrWhiteSpace(secureHash))
            throw new O24OpenAPIException("MISSING_VNPAY_SECURE_HASH");

        if (!vnpay.ValidateSignature(secureHash, cfg.HashSecret))
            throw new O24OpenAPIException("INVALID_VNPAY_SIGNATURE");

        string responseCode = vnpParams.GetValueOrDefault("vnp_ResponseCode")?.Trim();

        string responseCodeTask = await vNPayResponseCodeMapRepository.GetByResponseCodeAsync(
            responseCode,
            cancellationToken
        );

        string transactionStatus = vnpParams.GetValueOrDefault("vnp_TransactionStatus")?.Trim();
        string txStatusTask = await vNPayTransactionStatusMapRepository.GetByStatusCodeAsync(
            transactionStatus,
            cancellationToken
        );

        string responseCodeMap = responseCodeTask;
        string txStatusMap = txStatusTask;
        string transactionRef = vnpParams.GetValueOrDefault("vnp_TxnRef")?.Trim();
        string transactionDate = vnpParams.GetValueOrDefault("vnp_PayDate")?.Trim();

        return new VNPayProcessReturnResponseModel
        {
            TransactionRef = transactionRef,
            TransactionStatus = transactionStatus,
            TransactionStatusMessage =
                txStatusMap ?? "Không tìm thấy mô tả TransactionStatus trong DB",
            ResponseCodeStatus = responseCode,
            ResponseCodeMessage = responseCodeMap ?? "Không tìm thấy mô tả ResponseCode trong DB",
            TransactionDate = transactionDate,
        };
    }
}
