using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Test.Features
{
    public sealed class ZaloZnsClient
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ZaloTokenProvider _tokenProvider;
        private readonly IOptionsMonitor<ZaloOptions> _opt;

        public ZaloZnsClient(
            IHttpClientFactory httpFactory,
            ZaloTokenProvider tokenProvider,
            IOptionsMonitor<ZaloOptions> opt
        )
        {
            _httpFactory = httpFactory;
            _tokenProvider = tokenProvider;
            _opt = opt;
        }

        public async Task<string> SendOtpAsync(
            string phoneE164NoPlus,
            string otp,
            CancellationToken ct = default
        )
        {
            var o = _opt.CurrentValue;
            var accessToken = await _tokenProvider.GetAccessTokenAsync(ct);

            // POST https://business.openapi.zalo.me/message/template + header access_token :contentReference[oaicite:8]{index=8}
            var payload = new
            {
                mode = o.Mode, // "development" khi test
                phone = phoneE164NoPlus, // ví dụ: 84987654321
                template_id = o.TemplateId,
                template_data = new
                {
                    otp = otp, // key này phụ thuộc template của em
                },
                tracking_id = Guid.NewGuid().ToString("N")[..24],
            };

            var client = _httpFactory.CreateClient();
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                "https://business.openapi.zalo.me/message/template"
            );
            req.Headers.Add("access_token", accessToken);
            req.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var res = await client.SendAsync(req, ct);
            var body = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Send ZNS failed: {(int)res.StatusCode} {body}"
                );

            return body;
        }
    }
}
