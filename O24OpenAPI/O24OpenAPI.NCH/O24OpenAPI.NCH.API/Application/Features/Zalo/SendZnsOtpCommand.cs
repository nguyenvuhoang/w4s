using LinKit.Core.Cqrs;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo
{
    public class SendZnsOtpCommand : BaseO24OpenAPIModel, ICommand<bool>
    {
        public string RefId { get; set; } = default!;
        public string OaId { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string TemplateId { get; set; } = default!;

        public Dictionary<string, object> TemplateData { get; set; } = [];
    }

    [CqrsHandler]
    public class SendZnsOtpHandler(
    IZaloOATokenRepository tokenRepo,
    IHttpClientFactory httpClientFactory,
    IMediator mediator,
    IZaloZNSSendoutRepository sendoutRepo
) : ICommandHandler<SendZnsOtpCommand, bool>
    {
        private static readonly TimeSpan SafetyWindow = TimeSpan.FromMinutes(5);

        public async Task<bool> HandleAsync(SendZnsOtpCommand request, CancellationToken ct = default)
        {
            // 0) Idempotency: RefId unique
            if (await sendoutRepo.ExistsByRefIdAsync(request.RefId, ct))
                return true;

            // 1) Get token
            var token = await tokenRepo.GetActiveByOaIdAsync(request.OaId, ct)
                ?? throw new InvalidOperationException($"No active Zalo token for OA {request.OaId}");

            //// 2) Refresh if near expiry
            //if (token.ExpiresAtUtc <= DateTime.UtcNow.Add(SafetyWindow))
            //    token = await mediator.SendAsync(new RefreshZaloTokenCommand { OaId = request.OaId }, ct);

            // 3) Build payload 
            var payload = new
            {
                phone = request.Phone,
                template_id = request.TemplateId,
                template_data = request.TemplateData,
                tracking_id = request.RefId
            };

            var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload);

            // 4) Try send (1st attempt)
            var sendResult = await TrySendAsync(token.AccessToken, payloadJson, ct);

            //// 5) If unauthorized/token expired => refresh + retry once
            //if (sendResult.ShouldRefreshToken)
            //{
            //    token = await mediator.Send(new RefreshZaloTokenCommand { OaId = request.OaId }, ct);
            //    sendResult = await TrySendAsync(token.AccessToken, payloadJson, ct);
            //}

            // 6) Log result
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
                ZaloMsgId = sendResult.ZaloMsgId
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
            var client = httpClientFactory.CreateClient("ZaloZns");
            using var http = new HttpRequestMessage(HttpMethod.Post, "<<<ZNS_ENDPOINT_HERE>>>");
            http.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            http.Content = new StringContent(payloadJson, System.Text.Encoding.UTF8, "application/json");

            var resp = await client.SendAsync(http, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return ZnsSendResult.Refresh("401", raw);

            if (!resp.IsSuccessStatusCode)
                return ZnsSendResult.Fail(((int)resp.StatusCode).ToString(), raw);

            // parse json -> msg id / error code if any
            // nếu success: Success=true
            return ZnsSendResult.Ok(zaloMsgId: null);
        }

        private record ZnsSendResult(bool Success, bool ShouldRefreshToken, string? ErrorCode, string? ErrorMessage, string? ZaloMsgId)
        {
            public static ZnsSendResult Ok(string? zaloMsgId) => new(true, false, null, null, zaloMsgId);
            public static ZnsSendResult Fail(string code, string msg) => new(false, false, code, msg, null);
            public static ZnsSendResult Refresh(string code, string msg) => new(false, true, code, msg, null);
        }
    }

}
