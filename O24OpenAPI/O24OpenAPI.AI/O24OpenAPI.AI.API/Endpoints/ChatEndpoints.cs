using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.AI;
using O24OpenAPI.AI.API.Application.Features.ChatClients;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Extensions;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.AI.API.Endpoints;

public record ChatRequestModel(string Message, string UserCode);

public record ChatResponseModel(string Data, bool Done);

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "api/chat",
            async (
                HttpContext context,
                [Microsoft.AspNetCore.Mvc.FromBody] ChatRequestModel request,
                ILogger<Program> logger,
                IChatClient chatClient,
                [FromKeyedServices(MediatorKey.AI)] IMediator mediator
            ) =>
            {
                // Quyết định trọng số
                string userMessage = request.Message;
                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    await context.Response.WriteAsync("event: error\ndata: Missing message\n\n");
                    await context.Response.Body.FlushAsync();
                    return;
                }

                IHttpResponseBodyFeature responseBodyFeature =
                    context.Features.Get<IHttpResponseBodyFeature>();
                responseBodyFeature?.DisableBuffering();

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "text/event-stream; charset=utf-8";
                context.Response.Headers.CacheControl = "no-cache, no-transform";
                context.Response.Headers.Connection = "keep-alive";
                context.Response.Headers["X-Accel-Buffering"] = "no";

                List<Microsoft.Extensions.AI.ChatMessage> chatHistory =
                [
                    new(ChatRole.System, PromptTemplates.System),
                    new(ChatRole.System, PromptTemplates.Product),
                    new(ChatRole.System, PromptTemplates.Role),
                ];
                List<ChatMessage> optimizedHistory = await mediator.SendAsync(
                    new GetOptimizedHistoryCommand { UserCode = request.UserCode }
                );
                chatHistory.AddRange(optimizedHistory);
                //var userContext = await userContextService.BuildPromptAsync(userId);
                //messages.Add(new(ChatRole.System, userContext));
                chatHistory.Add(new ChatMessage(ChatRole.User, userMessage));
                IAsyncEnumerable<ChatResponseUpdate> responseStream =
                    chatClient.GetStreamingResponseAsync(
                        chatHistory,
                        new ChatOptions { MaxOutputTokens = 1000, Temperature = 0.7f }
                    );
                StringBuilder fullAIResponse = new();

                await foreach (ChatResponseUpdate chunk in responseStream)
                {
                    if (context.RequestAborted.IsCancellationRequested)
                        break;

                    if (!string.IsNullOrEmpty(chunk.Text))
                    {
                        fullAIResponse.Append(chunk.Text);
                        var sseData = new { content = chunk.Text, done = false };

                        string json = sseData.ToSerialize();
                        await context.Response.WriteAsync($"data: {json}\n\n");
                        await context.Response.Body.FlushAsync();
                    }
                }
                await mediator.SendAsync(
                    new SaveChatCommand(request.UserCode, "user", request.Message)
                );
                await mediator.SendAsync(
                    new SaveChatCommand(request.UserCode, "assistant", fullAIResponse.ToString())
                );
                string finalJson = JsonSerializer.Serialize(new { done = true });
                await context.Response.WriteAsync($"data: {finalJson}\n\n");
                await context.Response.Body.FlushAsync();
            }
        );
    }
}
