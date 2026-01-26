using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace O24OpenAPI.AI.API.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/chat", async (
            HttpContext context,
            [FromQuery] string message,
            [FromQuery] string conversationId,
            ILogger<Program> logger) =>
        {
            // 1. Validate UID header
            if (!context.Request.Headers.TryGetValue("uid", out StringValues uidHeader))
            {
                logger.LogWarning("Missing uid header");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing uid header");
                return;
            }

            string jwtToken = uidHeader.ToString();
            logger.LogInformation("Received request - Message: {Message}, ConvId: {ConvId}, Token: {Token}",
                message, conversationId, jwtToken.Substring(0, 50) + "...");

            // 2. Setup SSE
            IHttpResponseBodyFeature responseBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();
            responseBodyFeature?.DisableBuffering();

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/event-stream; charset=utf-8";
            context.Response.Headers.CacheControl = "no-cache, no-transform";
            context.Response.Headers.Connection = "keep-alive";
            context.Response.Headers["X-Accel-Buffering"] = "no";

            // 3. Send initial ping
            await SendSseCommentAsync(context, "connected");
            logger.LogInformation("SSE connection established");

            try
            {
                // 4. Stream messages
                for (int i = 0; i < 10; i++)
                {
                    if (context.RequestAborted.IsCancellationRequested)
                    {
                        logger.LogWarning("Client disconnected at message {Index}", i);
                        break;
                    }

                    var responseMessage = new
                    {
                        id = i,
                        text = $"Message {i + 1}",
                        originalMessage = message,
                        conversationId = conversationId,
                        timestamp = DateTime.UtcNow.ToString("O")
                    };

                    await SendSseDataAsync(context, responseMessage, eventId: i.ToString());

                    logger.LogInformation("[{Time}] Sent message {Index}",
                        DateTime.UtcNow.ToString("HH:mm:ss.fff"), i);

                    await Task.Delay(1000, context.RequestAborted);
                }

                // 5. Send completion event
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
        });
    }

    private static async Task SendSseCommentAsync(HttpContext context, string comment)
    {
        byte[] data = Encoding.UTF8.GetBytes($": {comment}\n\n");
        await context.Response.Body.WriteAsync(data);
        await context.Response.Body.FlushAsync();
    }

    private static async Task SendSseDataAsync(
        HttpContext context,
        object data,
        string eventId = null,
        string eventName = null)
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

        string json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });

        sb.Append($"data: {json}\n\n");

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await context.Response.Body.WriteAsync(bytes);
        await context.Response.Body.FlushAsync();
    }

    private static async Task SendSseEventAsync(
        HttpContext context,
        string eventName,
        string message)
    {
        var sb = new StringBuilder();
        sb.Append($"event: {eventName}\n");
        sb.Append($"data: {message}\n\n");

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await context.Response.Body.WriteAsync(bytes);
        await context.Response.Body.FlushAsync();
    }
}