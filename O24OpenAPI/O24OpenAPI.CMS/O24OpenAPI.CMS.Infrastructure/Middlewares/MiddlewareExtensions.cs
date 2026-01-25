using Microsoft.AspNetCore.Builder;

namespace O24OpenAPI.CMS.Infrastructure.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseProxyAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ProxyAuthenticationMiddleware>();
    }
}
