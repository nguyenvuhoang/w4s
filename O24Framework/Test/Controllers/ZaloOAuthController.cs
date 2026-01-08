using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/zalo-oauth")]
    public class ZaloOAuthController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _cfg;

        public ZaloOAuthController(IMemoryCache cache, IHttpClientFactory http, IConfiguration cfg)
        {
            _cache = cache;
            _http = http;
            _cfg = cfg;
        }

        [HttpGet("start")]
        public IActionResult Start()
        {
            //Check before get appid and secret key:
            var appId = _cfg["Zalo:AppId"];
            var redirectUri = _cfg["Zalo:RedirectUri"];

            if (string.IsNullOrWhiteSpace(appId))
                return BadRequest("Missing config: Zalo:AppId");

            if (string.IsNullOrWhiteSpace(redirectUri))
                return BadRequest("Missing config: Zalo:RedirectUri");

            // PKCE
            var codeVerifier = Base64Url(RandomNumberGenerator.GetBytes(32));
            var codeChallenge = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier)));

            var state = Base64Url(RandomNumberGenerator.GetBytes(16));

            // lưu tạm verifier theo state (10 phút)
            _cache.Set($"zalo:pkce:{state}", codeVerifier, TimeSpan.FromMinutes(10));

            var url =
                "https://oauth.zaloapp.com/v4/oa/permission" +
                $"?app_id={Uri.EscapeDataString(appId)}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&state={Uri.EscapeDataString(state)}" +
                $"&code_challenge={Uri.EscapeDataString(codeChallenge)}";

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state, CancellationToken ct)
        {
            if (!_cache.TryGetValue($"zalo:pkce:{state}", out string? codeVerifier) || string.IsNullOrWhiteSpace(codeVerifier))
                return BadRequest("Invalid/expired state");

            var appId = _cfg["Zalo:AppId"]!;
            var secretKey = _cfg["Zalo:SecretKey"]!;

            // Đổi authorization code -> access_token + refresh_token
            // Endpoint token: https://oauth.zaloapp.com/v4/oa/access_token (Zalo OA)
            var client = _http.CreateClient();
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://oauth.zaloapp.com/v4/oa/access_token");
            req.Headers.Add("secret_key", secretKey);
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["app_id"] = appId,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["code_verifier"] = codeVerifier
            });

            var res = await client.SendAsync(req, ct);
            var body = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, body);

            // DEV: trả ra cho dễ copy (prod: lưu vào secret store)
            return Content(body, "application/json");
        }

        private static string Base64Url(byte[] bytes)
            => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}
