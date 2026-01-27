namespace O24OpenAPI.CMS.API.Application.Features.Zalo
{
    using LinKit.Core.Cqrs;
    using O24OpenAPI.CMS.Infrastructure.Configurations;
    using O24OpenAPI.Core.Caching;
    using O24OpenAPI.Core.Configuration;
    using System.Text.Json;

    public class ZaloExchangeTokenCommand : BaseO24OpenAPIModel, ICommand<ExchangeZaloTokenResponse>
    {
        public string OaId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string? State { get; set; } = default!;
    }

    public class ExchangeZaloTokenResponse : BaseO24OpenAPIModel
    {
        public string AccessToken { get; set; } = default!;
        public string ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
    }

    [CqrsHandler]
    public class ExchangeZaloTokenHandler(
        IHttpClientFactory httpClientFactory,
        IStaticCacheManager staticCacheManager
    ) : ICommandHandler<ZaloExchangeTokenCommand, ExchangeZaloTokenResponse>
    {

        private readonly ZaloConfiguration zaloConfiguration =
        Singleton<AppSettings>.Instance?.Get<ZaloConfiguration>()
        ?? throw new InvalidOperationException("ZaloConfiguration is missing.");


        public async Task<ExchangeZaloTokenResponse> HandleAsync(ZaloExchangeTokenCommand request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.State))
                throw new InvalidOperationException("Missing state");
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new InvalidOperationException("Missing code");

            var cacheKey = new CacheKey($"ZALO:PKCE:{request.State}");

            var codeVerifier = await staticCacheManager.Get<string>(cacheKey);

            if (string.IsNullOrWhiteSpace(codeVerifier))
                throw new InvalidOperationException("Invalid or expired PKCE state (code_verifier not found)");

            await staticCacheManager.RemoveAsync(cacheKey);

            var appId = zaloConfiguration.AppId ?? throw new InvalidOperationException("Missing ZaloConfiguration:AppId");
            var secretKey = zaloConfiguration.SecretKey ?? throw new InvalidOperationException("Missing ZaloConfiguration:SecretKey");

            var client = httpClientFactory.CreateClient();

            using var http = new HttpRequestMessage(HttpMethod.Post, "https://oauth.zaloapp.com/v4/oa/access_token");
            http.Headers.TryAddWithoutValidation("secret_key", secretKey);
            http.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = request.Code,
                ["app_id"] = appId,
                ["grant_type"] = "authorization_code",
                ["code_verifier"] = codeVerifier
            });

            var resp = await client.SendAsync(http, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Zalo /access_token failed: {(int)resp.StatusCode} - {raw}");

            using var doc = JsonDocument.Parse(raw);

            var accessToken = doc.RootElement.GetProperty("access_token").GetString() ?? "";
            var expiresIn = doc.RootElement.TryGetProperty("expires_in", out var ei) ? ei.GetString() : null;
            var refreshToken = doc.RootElement.TryGetProperty("refresh_token", out var rt) ? rt.GetString() : null;

            if (string.IsNullOrWhiteSpace(accessToken))
                throw new Exception($"Zalo response missing access_token: {raw}");

            return new ExchangeZaloTokenResponse
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                RefreshToken = refreshToken
            };
        }
    }
}
