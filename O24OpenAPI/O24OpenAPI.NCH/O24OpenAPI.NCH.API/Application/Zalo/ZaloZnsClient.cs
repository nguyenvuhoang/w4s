using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public class ZaloZnsClient : IZaloZnsClient
{
    private readonly HttpClient _httpClient;
    private readonly ZaloZnsOptions _options;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public ZaloZnsClient(HttpClient httpClient, IOptions<ZaloZnsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<ZaloSendOtpResult> SendOtpAsync(
        string phoneNumber,
        string otp,
        string trackingId,
        CancellationToken cancellationToken = default
    )
    {
        var msisdn = NormalizePhone(phoneNumber);

        var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.SendOtpPath}";

        var payload = new
        {
            mode = _options.DevelopmentMode ? "development" : "unknown",
            oa_id = _options.OaId,
            phone = msisdn,
            sending_mode = "default",
            template_id = _options.OtpTemplateId,
            template_data = new { otp = otp, expired_time = "5" },
            tracking_id = trackingId,
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _options.AccessToken
        );

        request.Content = JsonContent.Create(payload);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = new ZaloSendOtpResult
        {
            StatusCode = (int)response.StatusCode,
            RawResponse = content,
            TrackingId = trackingId,
        };

        if (!response.IsSuccessStatusCode)
        {
            result.Success = false;
            result.ErrorMessage = $"HTTP error {response.StatusCode}";
            return result;
        }

        try
        {
            var zaloResponse = JsonSerializer.Deserialize<ZaloZnsSendResponse>(
                content,
                _jsonOptions
            );

            // error_code = 0 là thành công theo nhiều partner docs
            result.Success = zaloResponse?.error_code == 0;
            result.ErrorCode = zaloResponse?.error_code;
            result.ErrorMessage = zaloResponse?.error_message;
            result.ZnsMessageId = zaloResponse?.msg_id;

            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Cannot parse Zalo response: {ex.Message}";
            return result;
        }
    }

    private static string NormalizePhone(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return phoneNumber;

        var p = phoneNumber.Trim();

        if (p.StartsWith("+"))
            p = p[1..];

        if (p.StartsWith("0"))
            return "84" + p[1..];

        return p;
    }

    private class ZaloZnsSendResponse
    {
        public int error_code { get; set; }
        public string? error_message { get; set; }
        public string? msg_id { get; set; }
    }
}
