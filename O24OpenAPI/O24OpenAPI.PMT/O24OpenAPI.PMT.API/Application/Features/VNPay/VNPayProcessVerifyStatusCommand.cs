using LinKit.Core.Cqrs;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.PMT.API.Application.Helpers;
using O24OpenAPI.PMT.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.PMT.API.Application.Features.VNPay;

public class VNPayProcessVerifyStatusCommand
    : BaseTransactionModel,
        ICommand<VNPayProcessVerifyStatusResponseModel>
{
    public string TransactionRef { get; set; }
    public string TransactionDescription { get; set; } = default!;
    public string VNPayTransactionDate { get; set; } = default!;

}

public class VNPayProcessVerifyStatusResponseModel : BaseO24OpenAPIModel
{
    public string Status { get; set; } = default!;
    public string TransactionRef { get; set; } = default!;
}

public class VNPayQueryDrResponse
{
    public string vnp_ResponseCode { get; set; } = default!;
    public string vnp_Message { get; set; } = default!;
    public string vnp_TransactionStatus { get; set; } = default!;
    public string vnp_TxnRef { get; set; } = default!;
}


[CqrsHandler]
public class VNPayProcessVerifyStatusCommandHandle(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<VNPayProcessVerifyStatusCommand, VNPayProcessVerifyStatusResponseModel>
{
    public async Task<VNPayProcessVerifyStatusResponseModel> HandleAsync(
    VNPayProcessVerifyStatusCommand request,
    CancellationToken cancellationToken = default
)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new O24OpenAPIException("HttpContext is not available.");

        var ipAddr = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";

        var vnp_RequestId = DateTime.Now.Ticks.ToString();
        var cfg = Singleton<AppSettings>.Instance.Get<VNPayConfiguration>();

        var vnp_Api = cfg.VnpApi;
        var vnp_TmnCode = cfg.TmnCode;
        var vnp_HashSecret = cfg.HashSecret;
        var vnp_Version = cfg.Version;
        var vnp_Command = "querydr";

        var vnp_TxnRef = request.TransactionRef.Trim();

        var vnp_OrderInfo = string.IsNullOrWhiteSpace(request.TransactionDescription)
            ? $"Truy van giao dich: {vnp_TxnRef}"
            : request.TransactionDescription.Trim();


        var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");

        var vnp_TransactionDate = string.IsNullOrWhiteSpace(request.VNPayTransactionDate)
            ? vnp_CreateDate
            : request.VNPayTransactionDate.Trim();

        var vnp_IpAddr = ipAddr;

        var signData = $"{vnp_RequestId}|{vnp_Version}|{vnp_Command}|{vnp_TmnCode}|{vnp_TxnRef}|{vnp_TransactionDate}|{vnp_CreateDate}|{vnp_IpAddr}|{vnp_OrderInfo}";

        var vnp_SecureHash = VnPayUtils.HmacSHA512(vnp_HashSecret, signData);

        var qdrData = new Dictionary<string, string>
        {
            ["vnp_RequestId"] = vnp_RequestId,
            ["vnp_Version"] = vnp_Version,
            ["vnp_Command"] = vnp_Command,
            ["vnp_TmnCode"] = vnp_TmnCode,
            ["vnp_TxnRef"] = vnp_TxnRef,
            ["vnp_OrderInfo"] = vnp_OrderInfo,
            ["vnp_TransactionDate"] = vnp_TransactionDate,
            ["vnp_CreateDate"] = vnp_CreateDate,
            ["vnp_IpAddr"] = vnp_IpAddr,
            ["vnp_SecureHash"] = vnp_SecureHash
        };


        var responseJson = await PostAsync(vnp_Api, qdrData, cancellationToken);

        var vnp = JsonSerializer.Deserialize<VNPayQueryDrResponse>(
            responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? throw new O24OpenAPIException("VNPAY_QUERYDR_EMPTY_RESPONSE", new { Body = responseJson });

        var finalStatus = (vnp.vnp_ResponseCode == "00" && vnp.vnp_TransactionStatus == "00") ? "Success" : (vnp.vnp_TransactionStatus == "01" ? "Pending" : "Failed");

        return new VNPayProcessVerifyStatusResponseModel
        {
            TransactionRef = vnp_TxnRef,
            Status = finalStatus
        };
    }



    public async Task<string> PostAsync(
        string url,
        object payload,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var client = httpClientFactory.CreateClient(
                nameof(VNPayProcessVerifyStatusCommandHandle)
            );

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            client.Timeout = TimeSpan.FromSeconds(30);

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new O24OpenAPIException(
                    $"VNPAY_HTTP_ERROR: {(int)response.StatusCode} - {response.ReasonPhrase}",
                    new
                    {
                        response.StatusCode,
                        Body = errorBody,
                        Url = url
                    }
                );
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            // Timeout
            throw new O24OpenAPIException(
                "VNPAY_TIMEOUT",
                new
                {
                    Url = url,
                    TimeoutSeconds = 30
                },
                ex
            );
        }
        catch (HttpRequestException ex)
        {
            throw new O24OpenAPIException(
                "VNPAY_HTTP_REQUEST_FAILED",
                new
                {
                    Url = url,
                    ex.Message
                },
                ex
            );
        }
        catch (JsonException ex)
        {
            throw new O24OpenAPIException(
                "VNPAY_JSON_ERROR",
                new
                {
                    Url = url,
                    Payload = payload
                },
                ex
            );
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(
                "VNPAY_UNEXPECTED_ERROR",
                new
                {
                    Url = url,
                    Payload = payload
                },
                ex
            );
        }
    }


}
