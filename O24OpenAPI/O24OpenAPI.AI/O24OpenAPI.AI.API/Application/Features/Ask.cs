using LinKit.Core.Cqrs;
using O24OpenAPI.AI.API.Application.Utils;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace O24OpenAPI.AI.API.Application.Features;

public class AskCommand : BaseTransactionModel, ICommand<AskResponse>
{
    public string TenantId { get; set; } = default!;
    public string Question { get; set; } = default!;
    public string? DocType { get; set; }
    public string? LanguageFilter { get; set; }

    public int TopK { get; set; } = 5;
    public float MinScore { get; set; } = 0.70f;

    public string Collection { get; set; } = "o24_static_knowledge_v1";
}

public sealed record AskResponse(string Answer, IReadOnlyList<RagCitation> Citations);

public sealed record RagCitation(string PointId, float Score, string? DocId, string? Title);

[CqrsHandler]
public sealed class AskCommandHandler(QdrantClient qdrant)
    : ICommandHandler<AskCommand, AskResponse>
{
    private const int VectorSize = 1536;

    private readonly QdrantClient _qdrant = qdrant;

    public async Task<AskResponse> HandleAsync(
        AskCommand request,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrWhiteSpace(request.TenantId))
            throw new ArgumentException("TenantId is required.", nameof(request.TenantId));

        if (string.IsNullOrWhiteSpace(request.Question))
            throw new ArgumentException("Question is required.", nameof(request.Question));

        // 1) Embed (fake deterministic)
        var vector = Embedding.BuildFakeEmbedding(request.Question, VectorSize);

        // 2) Build filter
        var filter = new Filter();
        filter.Must.Add(
            new Condition
            {
                Field = new FieldCondition
                {
                    Key = "tenant_id",
                    Match = new Match { Keyword = request.TenantId },
                },
            }
        );

        if (!string.IsNullOrWhiteSpace(request.DocType))
        {
            filter.Must.Add(
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

        if (!string.IsNullOrWhiteSpace(request.LanguageFilter))
        {
            filter.Must.Add(
                new Condition
                {
                    Field = new FieldCondition
                    {
                        Key = "language",
                        Match = new Match { Keyword = request.LanguageFilter },
                    },
                }
            );
        }

        // 3) Search
        var results = await _qdrant.SearchAsync(
            collectionName: request.Collection,
            vector: vector,
            filter: filter,
            limit: (ulong)Math.Max(1, request.TopK),
            cancellationToken: ct
        );

        // 4) Apply threshold + map citations
        var filtered = results.Where(x => x.Score >= request.MinScore).ToList();

        if (filtered.Count == 0)
        {
            return new AskResponse(
                Answer: "Mình chưa tìm thấy thông tin phù hợp trong kho tài liệu hiện tại. Bạn thử hỏi cụ thể hơn hoặc kiểm tra lại doc_type/language/tenant nhé.",
                Citations: []
            );
        }

        var citations = filtered
            .Select(p =>
            {
                var id = p.Id?.Uuid ?? p.Id?.Num.ToString() ?? "";

                Value? docIdVal = null;
                Value? titleVal = null;

                if (p.Payload is not null)
                {
                    p.Payload.TryGetValue("doc_id", out docIdVal);
                    p.Payload.TryGetValue("title", out titleVal);
                }

                return new RagCitation(
                    PointId: id,
                    Score: p.Score,
                    DocId: docIdVal?.StringValue,
                    Title: titleVal?.StringValue
                );
            })
            .ToList();

        // 5) Compose answer (rule-based)
        var top = filtered[0];

        Value? contentVal = null;
        Value? topTitleVal = null;
        Value? topDocIdVal = null;

        if (top.Payload is not null)
        {
            top.Payload.TryGetValue("content", out contentVal);
            top.Payload.TryGetValue("title", out topTitleVal);
            top.Payload.TryGetValue("doc_id", out topDocIdVal);
        }

        var content = contentVal?.StringValue ?? "";
        var title = topTitleVal?.StringValue;
        var docId = topDocIdVal?.StringValue;

        var snippet = TrimTo(content, 800);

        var answer =
            $"{snippet}\n\nNguồn: {(string.IsNullOrWhiteSpace(title) ? "Tài liệu" : title)}"
            + (string.IsNullOrWhiteSpace(docId) ? "" : $" ({docId})")
            + $" | score={top.Score:0.###}";

        return new AskResponse(answer, citations);
    }

    private static string TrimTo(string s, int max)
    {
        if (string.IsNullOrEmpty(s) || s.Length <= max)
            return s;
        return s[..max] + "...";
    }
}
