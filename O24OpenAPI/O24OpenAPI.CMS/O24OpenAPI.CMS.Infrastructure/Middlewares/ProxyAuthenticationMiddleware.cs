using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.CMS.Infrastructure.Middlewares;

public class ProxyAuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldValidate(context.Request.Path))
        {
            string token = context.Request.Headers["uid"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                BusinessLogHelper.Warning("Missing auth token for {Path}", context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    new { error = "Unauthorized", message = "Missing authentication token" }
                );

                return;
            }

            IJwtTokenService authService =
                context.RequestServices.GetRequiredService<IJwtTokenService>();

            bool isValid = authService.ValidateToken(token).IsValid;

            if (!isValid)
            {
                BusinessLogHelper.Warning("Invalid token for {Path}", context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    new { error = "Forbidden", message = "Invalid or expired token" }
                );

                return;
            }

            context.Request.Headers.Append("X-Gateway-Validated", "true");

            BusinessLogHelper.Info("Request validated for {Path}", context.Request.Path);
            if (context.Request.Path.StartsWithSegments("/api/chat"))
            {
                IHttpResponseBodyFeature? bufferingFeature =
                    context.Features.Get<IHttpResponseBodyFeature>();
                bufferingFeature?.DisableBuffering();
            }
        }

        await next(context);
    }

    private static bool ShouldValidate(PathString path)
    {
        return path.StartsWithSegments("/api/chat") || path.StartsWithSegments("/api/proxy");
    }
}
