using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

namespace O24OpenAPI.AI.API.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/chat",
            async (HttpContext context, ILogger<Program> logger) =>
            {
                IHttpResponseBodyFeature responseBodyFeature =
                    context.Features.Get<IHttpResponseBodyFeature>();
                responseBodyFeature?.DisableBuffering();

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "text/event-stream; charset=utf-8";
                context.Response.Headers.CacheControl = "no-cache, no-transform";
                context.Response.Headers.Connection = "keep-alive";
                context.Response.Headers["X-Accel-Buffering"] = "no";

                await SendSseCommentAsync(context, "connected");

                logger.LogInformation("SSE connection established");

                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (context.RequestAborted.IsCancellationRequested)
                        {
                            logger.LogWarning("Client disconnected at message {Index}", i);
                            break;
                        }

                        var message = new
                        {
                            id = i,
                            text = $"Message {i + 1}",
                            timestamp = DateTime.UtcNow.ToString("O"),
                        };

                        await SendSseDataAsync(context, message, eventId: i.ToString());

                        logger.LogInformation(
                            "[{Time}] Sent message {Index}",
                            DateTime.UtcNow.ToString("HH:mm:ss.fff"),
                            i
                        );

                        await Task.Delay(1000, context.RequestAborted);
                    }

                    await SendSseEventAsync(context, "complete", "Stream finished");
                    logger.LogInformation("SSE stream completed successfully");
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("SSE stream cancelled by client");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during SSE streaming");
                    await SendSseEventAsync(context, "error", ex.Message);
                }
            }
        );
    }

    /// <summary>
    /// Send SSE comment (keep-alive)
    /// </summary>
    private static async Task SendSseCommentAsync(HttpContext context, string comment)
    {
        byte[] data = Encoding.UTF8.GetBytes($": {comment}\n\n");
        await context.Response.Body.WriteAsync(data);
        await context.Response.Body.FlushAsync();
    }

    /// <summary>
    /// Send SSE data event
    /// </summary>
    private static async Task SendSseDataAsync(
        HttpContext context,
        object data,
        string eventId = null,
        string eventName = null
    )
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(eventId))
        {
            sb.Append($"id: {eventId}\n");
        }

        if (!string.IsNullOrEmpty(eventName))
        {
            sb.Append($"event: {eventName}\n");
        }

        string json = JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        sb.Append($"data: {json}\n\n");

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await context.Response.Body.WriteAsync(bytes);
        await context.Response.Body.FlushAsync();
    }

    /// <summary>
    /// Send SSE named event
    /// </summary>
    private static async Task SendSseEventAsync(
        HttpContext context,
        string eventName,
        string message
    )
    {
        var sb = new StringBuilder();
        sb.Append($"event: {eventName}\n");
        sb.Append($"data: {message}\n\n");

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await context.Response.Body.WriteAsync(bytes);
        await context.Response.Body.FlushAsync();
    }
}
