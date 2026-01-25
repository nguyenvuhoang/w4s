using LinKit.Json.Runtime;
using Microsoft.AspNetCore.Http;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Enums;
using Serilog;
using System.Diagnostics;

namespace O24OpenAPI.Logging.Middlewares;

/// <summary>
/// Middleware to log details of incoming REST API requests and their responses.
/// </summary>
public class RestApiLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (
            !context.Request.Path.StartsWithSegments("/api")
            || context.Request.Path.StartsWithSegments("/api/chat")
            || context.Response.ContentType?.Contains("text/event-stream") == true
        )
        {
            await _next(context);
            return;
        }

        string correlationId =
            context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? EngineContext.Current.ResolveRequired<WorkContext>().ExecutionLogId;
        context.Items["CorrelationId"] = correlationId;

        Stopwatch stopwatch = Stopwatch.StartNew();
        context.Request.EnableBuffering();
        string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        var originalBodyStream = context.Response.Body;
        using MemoryStream responseBodyStream = new();
        context.Response.Body = responseBodyStream;
        Exception? exception = null;
        Dictionary<string, string> headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
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
            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
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
        var request = context.Request;

        string fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        string? prettyRequest = TryPrettifyJson(requestBody);
        string? prettyResponse = TryPrettifyJson(responseBody);

        var logger = Log.ForContext("LogType", LogType.RestApi)
            .ForContext("Direction", LogDirection.In)
            .ForContext("Action", fullUrl)
            .ForContext("Request", prettyRequest)
            .ForContext("Response", prettyResponse)
            .ForContext("Error", exception)
            .ForContext("Duration", duration)
            .ForContext("Headers", headers.WriteIndentedJson())
            .ForContext(
                "Flow",
                headers is not null && headers.TryGetValue("Flow", out string? flowValue)
                    ? flowValue.ToString()
                    : null
            );
        if (exception is not null)
        {
            logger.Error(exception, exception.Message);
        }
        else
        {
            logger.Information("REST API Call Log");
        }
    }

    private static string? TryPrettifyJson(string? jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            return jsonString;
        }

        try
        {
            object? dataObject = jsonString.FromJson<object>();
            return dataObject.WriteIndentedJson();
        }
        catch
        {
            return jsonString;
        }
    }
}
