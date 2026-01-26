namespace O24OpenAPI.AI.API.Application.Abstractions
{
    public interface IEmbeddingProvider
    {
        Task<float[]> EmbedAsync(string text, CancellationToken ct = default);
        Task<int> GetDimAsync(CancellationToken ct = default);
    }
}
