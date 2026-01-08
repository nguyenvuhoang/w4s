using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Test.Features
{
    public sealed class ZaloTokenProvider
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IMemoryCache _cache;
        private readonly IOptionsMonitor<ZaloOptions> _opt;

        private static readonly string CacheKey = "zalo_oa_access_token";

        public ZaloTokenProvider(
            IHttpClientFactory httpFactory,
            IMemoryCache cache,
            IOptionsMonitor<ZaloOptions> opt
        )
        {
            _httpFactory = httpFactory;
            _cache = cache;
            _opt = opt;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
        {
            if (
                _cache.TryGetValue(CacheKey, out string? token) && !string.IsNullOrWhiteSpace(token)
            )
                return token;

            var o = _opt.CurrentValue;

            // refresh token -> access token
            // POST https://oauth.zaloapp.com/v4/oa/access_token (x-www-form-urlencoded) + header secret_key :contentReference[oaicite:6]{index=6}
            var client = _httpFactory.CreateClient();
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                "https://oauth.zaloapp.com/v4/oa/access_token"
            );
            req.Headers.Add("secret_key", o.SecretKey);
            req.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["refresh_token"] = o.RefreshToken,
                    ["app_id"] = o.AppId,
                    ["grant_type"] = "refresh_token",
                }
            );

            var res = await client.SendAsync(req, ct);
            var body = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Refresh OA token failed: {(int)res.StatusCode} {body}"
                );
            }

            // Parse an toàn: check property tồn tại
            using var doc = System.Text.Json.JsonDocument.Parse(body);
            var root = doc.RootElement;

            if (!root.TryGetProperty("access_token", out var atEl))
            {
                // Zalo thường trả error dạng khác -> log body ra để biết
                throw new InvalidOperationException(
                    $"Refresh OA token response missing access_token. Body: {body}"
                );
            }

            var accessToken = atEl.GetString();
            var expiresIn = root.TryGetProperty("expires_in", out var exp) ? exp.GetInt32() : 3600;

            var newRefresh = root.TryGetProperty("refresh_token", out var rt)
                ? rt.GetString()
                : null;

            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException($"Empty access_token. Body: {body}");

            _cache.Set(CacheKey, accessToken, TimeSpan.FromSeconds(Math.Max(60, expiresIn - 120)));

            if (!string.IsNullOrWhiteSpace(newRefresh))
            {
                Console.WriteLine($"[ZALO] NEW_REFRESH_TOKEN = {newRefresh}");
            }

            return accessToken!;
        }
    }
}
