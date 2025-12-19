namespace O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate;

public sealed class UpsertPointRequest
{
    public string PointId { get; set; } = Guid.NewGuid().ToString("N");
    public string TenantId { get; set; } = default!;
    public string DocType { get; set; } = "unknown";
    public string Language { get; set; } = "vi";
    public string DocId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Dictionary<string, object>? Extra { get; set; }
}
