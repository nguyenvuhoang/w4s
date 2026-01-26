using O24OpenAPI.AI.API.Application.Models;

namespace O24OpenAPI.AI.API.Application.Abstractions;

public interface IQdrantService
{
    Task EnsureCollectionAsync(int vectorSize, CancellationToken ct = default);
    Task UpsertAsync(IEnumerable<VecPoint> points, CancellationToken ct = default);
    Task<List<VecHit>> SearchAsync(float[] query, int topK, CancellationToken ct = default);
}
