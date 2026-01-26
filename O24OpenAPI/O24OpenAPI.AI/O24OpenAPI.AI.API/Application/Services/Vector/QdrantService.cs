using Grpc.Core;
using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;
using O24OpenAPI.AI.API.Application.Models;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace O24OpenAPI.AI.API.Application.Services.Vector;

[RegisterService(Lifetime.Scoped)]
public class QdrantService : IQdrantService
{
    private readonly HttpClient _httpClientFactory;

    private readonly QdrantSettingConfig qdrantSettingConfig =
        Singleton<AppSettings>.Instance?.Get<QdrantSettingConfig>()
        ?? throw new InvalidOperationException("QdrantSettingConfig is missing.");

    private readonly QdrantClient qdrant = EngineContext.Current.Resolve<QdrantClient>();

    public QdrantService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory.CreateClient(nameof(QdrantService));

        var scheme = "https";
        var baseUrl = $"{scheme}://{qdrantSettingConfig.Host.TrimEnd('/')}:{qdrantSettingConfig.Port}/";

        _httpClientFactory.BaseAddress = new Uri(baseUrl);
    }


    private string? _activeCollection;

    public async Task EnsureCollectionAsync(int vectorSize, CancellationToken ct = default)
    {
        var name = $"{qdrantSettingConfig!.Collection}_{vectorSize}";
        _activeCollection = name;

        try
        {
            await qdrant.GetCollectionInfoAsync(name, ct);
            return; // exists
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            await qdrant.CreateCollectionAsync(
                collectionName: name,
                vectorsConfig: new VectorParams
                {
                    Size = (ulong)vectorSize,
                    Distance = Distance.Cosine
                },
                cancellationToken: ct
            );

            return;
        }
    }

    // Nếu IQdrantService của bạn cần lấy active collection:
    public string? GetActiveCollection() => _activeCollection;

    public async Task<List<VecHit>> SearchAsync(float[] query, int topK, CancellationToken ct = default)
    {
        var name = _activeCollection ?? qdrantSettingConfig.Collection;
        var body = new
        {
            vector = query,
            limit = topK,
            with_payload = true
        };
        var resp = await _httpClientFactory.PostAsJsonAsync($"collections/{name}/points/search", body, ct);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<QdrantSearchResponse>(cancellationToken: ct);

        var results = new List<VecHit>();
        foreach (var r in json.Result)
        {
            results.Add(new VecHit
            {
                Id = r.Id.ToString(),
                Score = r.Score,
                Text = r.Payload.Text,
                Source = r.Payload.Source
            });
        }
        return results;
    }

    public async Task UpsertAsync(IEnumerable<VecPoint> points, CancellationToken ct = default)
    {
        var name = _activeCollection ?? qdrantSettingConfig.Collection;
        var payload = new
        {
            points = points.Select(p => new
            {
                id = p.Id,
                vector = p.Vector,
                payload = new { text = p.Text, source = p.Source }
            })
        };
        var resp = await _httpClientFactory.PutAsJsonAsync($"collections/{name}/points?wait=true", payload, ct);
        resp.EnsureSuccessStatusCode();
    }
}
