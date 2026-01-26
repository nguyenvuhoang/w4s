using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using OpenAI.Embeddings;
using System.ClientModel;

namespace O24OpenAPI.AI.API.Application.Services.Embedding;

[RegisterService(Lifetime.Scoped)]
public class OpenAIEmbeddingProvider(EmbeddingClient client) : IEmbeddingProvider
{
    private readonly EmbeddingClient _client = client;
    private int? _dim;

    public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        ClientResult<OpenAIEmbedding> result = await _client.GenerateEmbeddingAsync(text, cancellationToken: ct);
        var vec = result.Value.ToFloats().ToArray();
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