using Microsoft.AspNetCore.Http;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Helpers;
using Serilog.Context;

namespace O24OpenAPI.Logging.Middlewares;

/// <summary>
/// Middleware to extract or generate a Correlation ID for each request
/// and push it to the LogContext.
/// </summary>
public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string HeaderKey = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[HeaderKey].FirstOrDefault() ?? Guid.NewGuid().ToString();
        EngineContext.Current.Resolve<WorkContext>().SetExecutionLogId(correlationId);

        // The ServiceName is pushed once at application startup in LoggingExtensions.
        // We only need to push the CorrelationId for each request scope.
        using (LogContext.PushProperty(LogContextHelper.CorrelationIdKey, correlationId))
        {
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(HeaderKey))
                {
                    context.Response.Headers.Append(HeaderKey, correlationId);
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
