using System.Text;
using System.Text.Json;
using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.AI;
using O24OpenAPI.AI.API.Application.AITools;
using O24OpenAPI.AI.API.Application.Features.ChatClients;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.AI.API.Endpoints;

public record ChatRequestModel(string ConversationId, string Message, string UserCode);

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
                [FromKeyedServices(MediatorKey.AI)] IMediator mediator,
                UserTools userTools
            ) =>
            {
                try
                {
                    if (!ValidateRequest(request))
                    {
                        await WriteSseError(context, "Missing message");
                        return;
                    }

                    ConfigureStreamingResponse(context);

                    List<ChatMessage> chatHistory = await BuildChatHistory(mediator, request);

                    IAsyncEnumerable<ChatResponseUpdate> responseStream = GetStreamingResponse(
                        chatClient,
                        chatHistory,
                        request,
                        userTools
                    );

                    string fullAIResponse = await StreamResponseToClient(context, responseStream);

                    await SaveChatHistory(mediator, request, fullAIResponse);

                    await SendCompletionSignal(context);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Chat request cancelled by client");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Chat SSE error");
                    await WriteSseError(
                        context,
                        "Đã xảy ra lỗi khi xử lý yêu cầu. Vui lòng thử lại."
                    );
                }
            }
        );
    }

    private static bool ValidateRequest(ChatRequestModel request)
    {
        return !string.IsNullOrWhiteSpace(request.Message);
    }

    private static void ConfigureStreamingResponse(HttpContext context)
    {
        IHttpResponseBodyFeature responseBodyFeature =
            context.Features.Get<IHttpResponseBodyFeature>();
        responseBodyFeature?.DisableBuffering();

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/event-stream; charset=utf-8";
        context.Response.Headers.CacheControl = "no-cache, no-transform";
        context.Response.Headers.Connection = "keep-alive";
        context.Response.Headers["X-Accel-Buffering"] = "no";
    }

    private static async Task<List<ChatMessage>> BuildChatHistory(
        IMediator mediator,
        ChatRequestModel request
    )
    {
        List<ChatMessage> chatHistory =
        [
            new(ChatRole.System, PromptTemplates.System),
            new(ChatRole.System, PromptTemplates.Product),
            new(ChatRole.System, PromptTemplates.Role),
        ];

        List<ChatMessage> optimizedHistory = await mediator.SendAsync(
            new GetOptimizedHistoryCommand
            {
                ConversationId = request.ConversationId,
                UserCode = request.UserCode,
            }
        );

        if (optimizedHistory.Count > 0)
        {
            chatHistory.AddRange(optimizedHistory);
        }

        chatHistory.Add(new ChatMessage(ChatRole.User, request.Message));

        return chatHistory;
    }

    private static IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponse(
        IChatClient chatClient,
        List<ChatMessage> chatHistory,
        ChatRequestModel request,
        UserTools userTools
    )
    {
        return chatClient.GetStreamingResponseAsync(
            chatHistory,
            new ChatOptions
            {
                MaxOutputTokens = 1000,
                Temperature = 0.7f,
                Tools = CreateAITools(request.UserCode, userTools),
            }
        );
    }

    private static List<AITool> CreateAITools(string userCode, UserTools userTools)
    {
        return
        [
            AIFunctionFactory.Create(
                async () => await userTools.GetUserBalance(userCode),
                "get_my_balance",
                "Lấy danh sách số dư của tôi"
            ),
            AIFunctionFactory.Create(
                async (string fromDate, string toDate) =>
                {
                    if (
                        !DateTime.TryParse(fromDate, out var from)
                        || !DateTime.TryParse(toDate, out var to)
                    )
                    {
                        return "Ngày không hợp lệ. Vui lòng dùng định dạng yyyy-MM-dd.";
                    }

                    return await userTools.GetUserSpending(userCode, from, to);
                },
                "get_my_spend",
                "Lấy danh sách chi tiêu của tôi. fromDate và toDate dùng yyyy-MM-dd"
            ),
        ];
    }

    private static async Task<string> StreamResponseToClient(
        HttpContext context,
        IAsyncEnumerable<ChatResponseUpdate> responseStream
    )
    {
        StringBuilder fullAIResponse = new();

        await foreach (ChatResponseUpdate chunk in responseStream)
        {
            if (context.RequestAborted.IsCancellationRequested)
                break;

            if (!string.IsNullOrEmpty(chunk.Text))
            {
                fullAIResponse.Append(chunk.Text);
                await SendSseChunk(context, chunk.Text, done: false);
            }
        }

        return fullAIResponse.ToString();
    }

    private static async Task SendSseChunk(HttpContext context, string content, bool done)
    {
        var sseData = new { content, done };
        string json = sseData.ToSerialize();
        await context.Response.WriteAsync($"data: {json}\n\n");
        await context.Response.Body.FlushAsync();
    }

    private static async Task SaveChatHistory(
        IMediator mediator,
        ChatRequestModel request,
        string aiResponse
    )
    {
        await mediator.SendAsync(
            new SaveChatCommand(request.ConversationId, request.UserCode, "user", request.Message)
        );

        await mediator.SendAsync(
            new SaveChatCommand(request.ConversationId, request.UserCode, "assistant", aiResponse)
        );
    }

    private static async Task SendCompletionSignal(HttpContext context)
    {
        string finalJson = JsonSerializer.Serialize(new { done = true });
        await context.Response.WriteAsync($"data: {finalJson}\n\n");
        await context.Response.Body.FlushAsync();
    }

    private static async Task WriteSseError(HttpContext context, string message)
    {
        var errorJson = JsonSerializer.Serialize(new { error = message, done = true });
        await context.Response.WriteAsync($"event: error\n");
        await context.Response.WriteAsync($"data: {errorJson}\n\n");
        await context.Response.Body.FlushAsync();
    }
}
