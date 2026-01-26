using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using O24OpenAPI.AI.API.Application.Models;

namespace O24OpenAPI.AI.API.Application.Services
{
    [RegisterService(Lifetime.Scoped)]
    public class RagPipelineService(IQdrantService qdrantService) : IRagPipelineService
    {
        private readonly IEmbeddingProvider _embeddingProvider;
        public async Task IngestAsync(IEnumerable<(string text, string source)> chunks, CancellationToken ct = default)
        {
            int dim = await _embeddingProvider.GetDimAsync(ct);

            await qdrantService.EnsureCollectionAsync(dim, ct);

            var list = new List<VecPoint>();
            foreach (var (text, source) in chunks)
            {
                var vec = await _embeddingProvider.EmbedAsync(text, ct);
                list.Add(new VecPoint
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Vector = vec,
                    Text = text,
                    Source = source
                });
            }
            await qdrantService.UpsertAsync(list, ct);
        }
    }
}
