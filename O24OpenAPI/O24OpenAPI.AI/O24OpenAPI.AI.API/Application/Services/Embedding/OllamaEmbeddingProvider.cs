using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.AI.API.Application.Services.Embedding;

[RegisterService(Lifetime.Scoped)]
public class OllamaEmbeddingProvider : IEmbeddingProvider
{
    private readonly LLMProviderConfig llmProviderConfig =
        Singleton<AppSettings>.Instance?.Get<LLMProviderConfig>()
        ?? throw new InvalidOperationException("LLMProviderConfig is missing.");
    private readonly HttpClient _http;
    private int? _dim;
    public OllamaEmbeddingProvider(IHttpClientFactory f)
    {
        _http = f.CreateClient(nameof(OllamaEmbeddingProvider));
        _http.BaseAddress = new Uri(llmProviderConfig.Ollama.BaseUrl);
    }

    public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        var body = new { model = llmProviderConfig.Ollama.EmbedModel, input = text };
        HttpResponseMessage resp = await _http.PostAsJsonAsync("api/embeddings", body, ct);
        resp.EnsureSuccessStatusCode();
        dynamic json = await resp.Content.ReadFromJsonAsync<dynamic>(cancellationToken: ct);
        var vec = ((IEnumerable<object>)json.embedding).Select(o => Convert.ToSingle(o)).ToArray();
        _dim ??= vec.Length;
        return vec;
    }

    public async Task<int> GetDimAsync(CancellationToken ct = default)
    {
        if (_dim.HasValue) return _dim.Value;
        _ = await EmbedAsync("dimension probe", ct);
        return _dim!.Value;
    }
}
