using O24OpenAPI.AI.API.Application.Abstractions;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.AI.API.Application.Services.LLM;

public class OllamaChatProvider : ILlmChatProvider
{
    private readonly HttpClient _http;
    private readonly LLMProviderConfig llmProviderConfig =
      Singleton<AppSettings>.Instance?.Get<LLMProviderConfig>()
      ?? throw new InvalidOperationException("LLMProviderConfig is missing.");
    public OllamaChatProvider(IHttpClientFactory f)
    {
        _http = f.CreateClient(nameof(OllamaChatProvider));
        _http.BaseAddress = new Uri(llmProviderConfig.Ollama.BaseUrl);
    }

    public async Task<string> AskAsync(string system, string user, CancellationToken ct = default)
    {
        var body = new
        {
            model = llmProviderConfig.Ollama.ChatModel,
            messages = new object[] {
                new { role="system", content=system },
                new { role="user", content=user }
            },
            stream = false,
            options = new { temperature = 0.2 }
        };
        var resp = await _http.PostAsJsonAsync("api/chat", body, ct);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<dynamic>(cancellationToken: ct);
        return (string)json.message.content;
    }
}

