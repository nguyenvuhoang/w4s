using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.CMS.Infrastructure.RateLimiting;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "[CMS] Too many requests. Please try again later.",
                    token
                );
            };

            options.AddPolicy(
                "api-rate-limit",
                httpContext =>
                {
                    string? path = httpContext.Request.Path.Value?.ToLower();

                    if (string.IsNullOrEmpty(path) || !path.StartsWith("/api/"))
                    {
                        return RateLimitPartition.GetNoLimiter("no-api");
                    }

                    string userKey =
                        httpContext.User?.Identity?.IsAuthenticated == true
                            ? httpContext.User.Identity!.Name!
                            : httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    int limit = RateLimitRules.GetLimit(path);

                    return RateLimitPartition.GetFixedWindowLimiter(
                        $"{path}:{userKey}",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = limit,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );
        });

        return services;
    }

    public static IApplicationBuilder UseApiRateLimiting(this IApplicationBuilder app)
    {
        return app.UseRateLimiter();
    }
}
