using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using OpenAI.Chat;

namespace O24OpenAPI.AI.API.Application.Services.LLM;

[RegisterService(Lifetime.Scoped)]
public class OpenAIChatProvider : ILlmChatProvider
{
    private readonly ChatClient _chat;

    public OpenAIChatProvider(ChatClient chat)
    {
        _chat = chat;
    }

    public async Task<string> AskAsync(string system, string user, CancellationToken ct = default)
    {
        List<ChatMessage> messages = new()
        {
            new SystemChatMessage(system),
            new UserChatMessage(user)
        };

        var completion = await _chat.CompleteChatAsync(messages, cancellationToken: ct);
        return completion.Value.Content[0].Text.ToString();
    }

}