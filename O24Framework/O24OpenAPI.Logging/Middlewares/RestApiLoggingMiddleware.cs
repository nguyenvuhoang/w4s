using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Enums;
using Serilog;

namespace O24OpenAPI.Logging.Middlewares;

/// <summary>
/// Middleware to log details of incoming REST API requests and their responses.
/// </summary>
public class RestApiLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        var correlationId =
            context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? EngineContext.Current.ResolveRequired<WorkContext>().ExecutionLogId;
        context.Items["CorrelationId"] = correlationId;

        var stopwatch = Stopwatch.StartNew();
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;
        Exception? exception = null;
        var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        finally
        {
            stopwatch.Stop();
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
            LogApiCall(
                context,
                requestBody,
                responseBody,
                exception,
                stopwatch.ElapsedMilliseconds,
                headers
            );
            if (exception is not null)
            {
                throw exception;
            }
        }
    }

    private static void LogApiCall(
        HttpContext context,
        string requestBody,
        string? responseBody,
        Exception? exception,
        long duration,
        IDictionary<string, string>? headers
    )
    {
        var action =
            $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
        var prettyRequest = TryPrettifyJson(requestBody);
        var prettyResponse = TryPrettifyJson(responseBody);

        Log.ForContext("LogType", LogType.RestApi)
            .ForContext("Direction", LogDirection.In)
            .ForContext("Action", action)
            .ForContext("Request", prettyRequest)
            .ForContext("Response", prettyResponse)
            .ForContext("Error", exception)
            .ForContext("Duration", duration)
            .ForContext("Headers", JsonConvert.SerializeObject(headers, Formatting.Indented))
            .ForContext(
                "Flow",
                headers is not null && headers.TryGetValue("Flow", out var flowValue)
                    ? flowValue.ToString()
                    : null
            )
            .Information("REST API Call Log");
    }

    private static string? TryPrettifyJson(string? jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            return jsonString;
        }

        try
        {
            var jObject = JObject.Parse(jsonString);
            return JsonConvert.SerializeObject(jObject, Formatting.Indented);
        }
        catch
        {
            return jsonString;
        }
    }
}
