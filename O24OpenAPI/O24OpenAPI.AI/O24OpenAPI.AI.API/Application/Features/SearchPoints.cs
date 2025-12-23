using LinKit.Core.Cqrs;
using O24OpenAPI.AI.API.Application.Utils;
using O24OpenAPI.Framework.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace O24OpenAPI.AI.API.Application.Features;

public class SearchPointsCommand : BaseTransactionModel, ICommand<SearchPointsResponse>
{
    public string TenantId { get; set; } = default!;
    public string QueryText { get; set; } = default!;
    public string Collection { get; set; } = "o24_static_knowledge_v1";
    public int TopK { get; set; } = 5;
    public string? DocType { get; set; }
    public new string? Language { get; set; } = "en";
    public bool UseNamedVector { get; set; } = false;
    public string VectorName { get; set; } = "text";
    public int VectorSize { get; set; } = 1536;
}

public sealed record SearchPointsResponse(IReadOnlyList<SearchHit> Hits);

public sealed record SearchHit(
    string PointId,
    float Score,
    string? DocId,
    string? Title,
    string? Content
);

[CqrsHandler]
public sealed class SearchPointsCommandHandler(QdrantClient qdrant)
    : ICommandHandler<SearchPointsCommand, SearchPointsResponse>
{
    private readonly QdrantClient _qdrant = qdrant;

    public async Task<SearchPointsResponse> HandleAsync(
        SearchPointsCommand request,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrWhiteSpace(request.TenantId))
            throw new ArgumentException("TenantId is required.", nameof(request.TenantId));

        if (string.IsNullOrWhiteSpace(request.QueryText))
            throw new ArgumentException("QueryText is required.", nameof(request.QueryText));

        // 1) embedding fake inline
        var vector = Embedding.BuildFakeEmbedding(request.QueryText, request.VectorSize);

        // 2) build filter: tenant_id must match, doc_type/language optional
        var must = new List<Condition>
        {
            new()
            {
                Field = new FieldCondition
                {
                    Key = "tenant_id",
                    Match = new Match { Keyword = request.TenantId },
                },
            },
        };

        if (!string.IsNullOrWhiteSpace(request.DocType))
        {
            must.Add(
                new Condition
                {
                    Field = new FieldCondition
                    {
                        Key = "doc_type",
                        Match = new Match { Keyword = request.DocType },
                    },
                }
            );
        }

        if (!string.IsNullOrWhiteSpace(request.Language))
        {
            must.Add(
                new Condition
                {
                    Field = new FieldCondition
                    {
                        Key = "language",
                        Match = new Match { Keyword = request.Language },
                    },
                }
            );
        }

        var filter = new Filter();
        filter.Must.AddRange(must);

        // 3) search
        IReadOnlyList<ScoredPoint> results;

        if (!request.UseNamedVector)
        {
            results = await _qdrant.SearchAsync(
                collectionName: request.Collection,
                vector: vector,
                filter: filter,
                limit: (ulong)request.TopK,
                cancellationToken: ct
            );
        }
        else
        {
            results = await _qdrant.SearchAsync(
                collectionName: request.Collection,
                vectorName: request.VectorName,
                vector: vector,
                filter: filter,
                limit: (ulong)request.TopK,
                cancellationToken: ct
            );
        }

        // 4) map payload
        var hits = results
            .Select(p =>
            {
                var id = p.Id?.Uuid ?? p.Id?.Num.ToString() ?? "";
                var payload = p.Payload;

                var docIdVal = payload?.GetValueOrDefault("doc_id");
                var titleVal = payload?.GetValueOrDefault("title");
                var contentVal = payload?.GetValueOrDefault("content");

                return new SearchHit(
                    id,
                    p.Score,
                    docIdVal?.StringValue,
                    titleVal?.StringValue,
                    contentVal?.StringValue
                );
            })
            .ToList();

        return new SearchPointsResponse(hits);
    }
}
