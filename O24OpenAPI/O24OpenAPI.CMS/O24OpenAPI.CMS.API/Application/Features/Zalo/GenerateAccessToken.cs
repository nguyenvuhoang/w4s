using LinKit.Core.Cqrs;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Core.Caching;
using System.Web;

namespace O24OpenAPI.CMS.API.Application.Features.Zalo;

public class GenerateAccessTokenCommand
    : BaseO24OpenAPIModel,
      ICommand<GenerateZaloAuthUrlResponse>
{
    public string? TenantId { get; set; }
    public string? ReturnTo { get; set; }
}

public class GenerateZaloAuthUrlResponse
{
    public string AuthorizationUrl { get; set; } = default!;
    public string State { get; set; } = default!;
}

[CqrsHandler]
public class GenerateAccessTokenHandler(
    IStaticCacheManager staticCacheManager,
    ZaloConfiguration zaloConfiguration
) : ICommandHandler<GenerateAccessTokenCommand, GenerateZaloAuthUrlResponse>
{
    private const string PKCE_CACHE_KEY = "ZALO:PKCE:";

    public async Task<GenerateZaloAuthUrlResponse> HandleAsync(
        GenerateAccessTokenCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var appId = zaloConfiguration.AppId
            ?? throw new InvalidOperationException("Missing ZaloConfiguration:AppId");

        var redirectUri = zaloConfiguration.RedirectUri
            ?? throw new InvalidOperationException("Missing ZaloConfiguration:RedirectUri");
        //  Generate PKCE
        var state = PkceUtil.GenerateState();
        var codeVerifier = PkceUtil.GenerateCodeVerifier();
        var codeChallenge = PkceUtil.GenerateCodeChallenge(codeVerifier);

        var cacheKey = new CacheKey(PKCE_CACHE_KEY);

        var cache = staticCacheManager.PrepareKeyForDefaultCache(cacheKey, state);
        cache.CacheTime = 10;

        await staticCacheManager.Set(cacheKey, codeVerifier);

        var qs = HttpUtility.ParseQueryString(string.Empty);
        qs["app_id"] = appId;
        qs["redirect_uri"] = redirectUri;
        qs["code_challenge"] = codeChallenge;
        qs["code_challenge_method"] = "S256";
        qs["state"] = state;

        var authorizationUrl = $"https://oauth.zaloapp.com/v4/oa/permission?{qs}";

        return new GenerateZaloAuthUrlResponse
        {
            AuthorizationUrl = authorizationUrl,
            State = state
        };
    }
}
