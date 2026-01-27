using LinKit.Core.Cqrs;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Framework.Extensions;
using System.Web;

namespace O24OpenAPI.CMS.API.Application.Features.Zalo;

public class GenerateAccessTokenCommand
    : BaseO24OpenAPIModel,
      ICommand<GenerateZaloAuthUrlResponse>
{ }

public class GenerateZaloAuthUrlResponse
{
    public string AuthorizationUrl { get; set; } = default!;
    public string State { get; set; } = default!;
}

[CqrsHandler]
public class GenerateAccessTokenHandler(IStaticCacheManager staticCacheManager)
    : ICommandHandler<GenerateAccessTokenCommand, GenerateZaloAuthUrlResponse>
{
    private readonly ZaloConfiguration zaloConfiguration =
        Singleton<AppSettings>.Instance?.Get<ZaloConfiguration>()
        ?? throw new InvalidOperationException("ZaloConfiguration is missing.");

    public async Task<GenerateZaloAuthUrlResponse> HandleAsync(
        GenerateAccessTokenCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var appId = zaloConfiguration.AppId
                ?? throw new InvalidOperationException("Missing ZaloConfiguration:AppId");

            var redirectUri = zaloConfiguration.RedirectUri
                ?? throw new InvalidOperationException("Missing ZaloConfiguration:RedirectUri");

            // Generate PKCE
            var state = PkceUtil.GenerateState();
            var codeVerifier = PkceUtil.GenerateCodeVerifier();
            var codeChallenge = PkceUtil.GenerateCodeChallenge(codeVerifier);

            var cacheKey = new CacheKey($"ZALO:PKCE:{state}");
            await staticCacheManager.Set(
                new CacheKey($"ZALO:PKCE:{state}") { CacheTime = 10 },
                codeVerifier
            );

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
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw new ApplicationException("Failed to generate Zalo authorization URL.", ex);

        }
    }
}
