using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;
using O24OpenAPI.NCH.Infrastructure.Configurations;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public class SendZnsOtpCommand : BaseTransactionModel, ICommand<bool>
{
    // required by Zalo API
    public string Phone { get; set; } = null!;
    public string TemplateId { get; set; } = null!;
    public string Otp { get; set; } = null!;
    // to lookup active token
    public string OaId { get; set; } = null!;
    public string? TrackingId { get; set; }
}

[CqrsHandler]
public class SendZnsOtpHandler(
    IZaloOATokenRepository tokenRepo,
    IHttpClientFactory httpClientFactory,
    IZaloZNSSendoutRepository sendoutRepo,
    O24NCHSetting o24NCHSetting
) : ICommandHandler<SendZnsOtpCommand, bool>
{

    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_SEND_ZBS_OTP)]
    public async Task<bool> HandleAsync(SendZnsOtpCommand request, CancellationToken ct = default)
    {
        // Idempotent by RefId (your internal key)
        if (await sendoutRepo.ExistsByRefIdAsync(request.RefId, ct))
            return true;

        var token = await tokenRepo.GetActiveByOaIdAsync(request.OaId, ct)
            ?? throw new InvalidOperationException($"No active Zalo token for OA {request.OaId}");

        var trackingId = string.IsNullOrWhiteSpace(request.TrackingId) ? request.RefId : request.TrackingId;

        var payload = new
        {
            phone = NormalizeZaloPhone(request.Phone),
            template_id = request.TemplateId,
            template_data = new Dictionary<string, string>
            {
                ["otp"] = request.Otp
            },
            tracking_id = trackingId
        };

        var payloadJson = JsonSerializer.Serialize(payload);

        // SEND
        var sendResult = await TrySendAsync(token.AccessToken, payloadJson, ct);

        // LOG
        await sendoutRepo.InsertAsync(new ZaloZNSSendout
        {
            RefId = request.RefId,
            OaId = request.OaId,
            Phone = request.Phone,
            TemplateId = request.TemplateId,
            PayloadJson = payloadJson,
            Status = sendResult.Success ? "SUCCESS" : "FAIL",
            ErrorCode = sendResult.ErrorCode,
            ErrorMessage = sendResult.ErrorMessage,
            ZaloMsgId = sendResult.ZaloMsgId,
            AttemptCount = 1,
            CreatedOnUtc = DateTime.UtcNow
        });

        if (sendResult.Success)
        {
            await tokenRepo.UpdateLastUsedAsync(token.OaId, ct);
            return true;
        }

        throw new Exception($"ZNS send failed: {sendResult.ErrorCode} - {sendResult.ErrorMessage}");
    }

    private async Task<ZnsSendResult> TrySendAsync(string accessToken, string payloadJson, CancellationToken ct)
    {
        var endpoint = o24NCHSetting.ZnsSendEndpoint ?? "https://business.openapi.zalo.me/message/template";

        var client = httpClientFactory.CreateClient(nameof(SendZnsOtpHandler));

        using var http = new HttpRequestMessage(HttpMethod.Post, endpoint);

        http.Headers.TryAddWithoutValidation("access_token", accessToken);
        http.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        http.Content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

        var resp = await client.SendAsync(http, ct);
        var raw = await resp.Content.ReadAsStringAsync(ct);

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            return ZnsSendResult.Refresh("401", raw);

        if (!resp.IsSuccessStatusCode)
            return ZnsSendResult.Fail(((int)resp.StatusCode).ToString(), raw);

        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            var error = root.TryGetProperty("error", out var errEl)
                ? (errEl.ValueKind == JsonValueKind.Number ? errEl.GetInt32() : int.TryParse(errEl.GetString(), out var e) ? e : -1)
                : -1;

            var message = root.TryGetProperty("message", out var msgEl) ? msgEl.GetString() : null;

            if (error != 0)
                return ZnsSendResult.Fail(error.ToString(), message ?? raw);

            string? msgId = null;
            string? sentTime = null;

            if (root.TryGetProperty("data", out var dataEl) && dataEl.ValueKind == JsonValueKind.Object)
            {
                if (dataEl.TryGetProperty("msg_id", out var midEl)) msgId = midEl.GetString();
                if (dataEl.TryGetProperty("sent_time", out var stEl)) sentTime = stEl.GetString();
            }

            return ZnsSendResult.Ok(msgId, sentTime);
        }
        catch
        {
            return ZnsSendResult.Ok(zaloMsgId: null, sentTime: null);
        }
    }

    private static string NormalizeZaloPhone(string phone)
    {
        var p = (phone ?? string.Empty).Trim();
        if (p.StartsWith('+')) p = p[1..];
        if (p.StartsWith('0')) p = "84" + p[1..];
        return p;
    }

    private record ZnsSendResult(
        bool Success,
        bool ShouldRefreshToken,
        string? ErrorCode,
        string? ErrorMessage,
        string? ZaloMsgId,
        string? SentTime
    )
    {
        public static ZnsSendResult Ok(string? zaloMsgId, string? sentTime) => new(true, false, null, null, zaloMsgId, sentTime);
        public static ZnsSendResult Fail(string code, string msg) => new(false, false, code, msg, null, null);
        public static ZnsSendResult Refresh(string code, string msg) => new(false, true, code, msg, null, null);
    };
}