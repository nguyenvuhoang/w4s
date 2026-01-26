using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using OpenAI.Chat;

namespace O24OpenAPI.AI.API.Application.Services.LLM;

[RegisterService(Lifetime.Scoped)]
public class OpenAIChatProvider(ChatClient chat) : ILlmChatProvider
{
    private readonly ChatClient _chat = chat;

    public async Task<string> AskAsync(string system, string user, CancellationToken ct = default)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(system),
            new UserChatMessage(user)
        };

        var completion = await _chat.CompleteChatAsync(messages, cancellationToken: ct);
        return completion.Value.Content[0].Text.ToString();
    }

}