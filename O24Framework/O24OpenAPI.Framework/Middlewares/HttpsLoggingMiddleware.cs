using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.Framework.Middlewares;

/// <summary>
/// The https logging middleware class
/// </summary>
public class HttpsLoggingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// /// The next
    /// </summary>
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    /// Invokes the context
    /// </summary>
    /// <param name="context">The context</param>
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.ContentType?.StartsWith("application/grpc") == true)
        {
            await _next(context);
            return;
        }

        var _httpLog = EngineContext.Current.Resolve<HttpLog>();
        _httpLog.HttpMethod = context.Request.Method;
        _httpLog.RequestUrl =
            $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        _httpLog.BeginOnUtc = DateTime.UtcNow;
        _httpLog.ClientIp = context.Connection.RemoteIpAddress?.ToString();

        // Capture original streams
        var originalBody = context.Request.Body;
        var originalResponseBody = context.Response.Body;

        var requestBodyStream = new MemoryStream();
        var responseBodyStream = new MemoryStream();

        try
        {
            // Only copy the request if it has content
            if (context.Request.ContentLength > 0)
            {
                await originalBody.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;
            }

            // Đọc raw body và gán vào Items trước khi _next
            using (var reader = new StreamReader(requestBodyStream, Encoding.UTF8, leaveOpen: true))
            {
                var rawBody = await reader.ReadToEndAsync();
                context.Items["RawRequestBody"] = rawBody;
            }

            // Reset lại stream để _next() dùng được
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            // Set the response body to our memory stream
            context.Response.Body = responseBodyStream;

            await _next(context);

            // Only set request headers if not already set
            if (string.IsNullOrEmpty(_httpLog.RequestHeaders))
            {
                _httpLog.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            }

            // Only read request body if not already set and if there's content
            if (string.IsNullOrEmpty(_httpLog.RequestBody) && context.Request.ContentLength > 0)
            {
                // Move to beginning of stream
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                using (
                    var reader = new StreamReader(requestBodyStream, Encoding.UTF8, leaveOpen: true)
                )
                {
                    var rawBody = await reader.ReadToEndAsync();

                    context.Items["RawRequestBody"] = rawBody;

                    _httpLog.RequestBody = rawBody;
                }
                requestBodyStream.Seek(0, SeekOrigin.Begin);
            }

            _httpLog.ResponseStatusCode = context.Response.StatusCode;

            // Only set response headers if not already set
            if (string.IsNullOrEmpty(_httpLog.ResponseHeaders))
            {
                _httpLog.ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers);
            }

            // Only read response body if not already set
            if (string.IsNullOrEmpty(_httpLog.ResponseBody))
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                using (
                    var reader = new StreamReader(
                        responseBodyStream,
                        Encoding.UTF8,
                        leaveOpen: true
                    )
                )
                {
                    _httpLog.ResponseBody = await reader.ReadToEndAsync();
                }
            }

            // Copy the response to the original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
        catch (Exception ex)
        {
            _httpLog.ResponseStatusCode = 500;
            _httpLog.ExceptionMessage = ex.Message;
        }
        finally
        {
            // Restore original streams
            context.Request.Body = originalBody;
            context.Response.Body = originalResponseBody;

            // Dispose the streams manually
            requestBodyStream.Dispose();
            responseBodyStream.Dispose();

            _httpLog.FinishOnUtc = DateTime.UtcNow;
            _ = TaskUtils.RunAsync(async () =>
            {
                await _httpLog.LogHttpAsync();
            });
        }
    }
}
