namespace O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate;

public sealed class SearchPointRequest
{
    public string TenantId { get; set; } = default!;
    public string QueryText { get; set; } = default!;
    public string? DocType { get; set; }
    public string? Language { get; set; } = "en";
    public int TopK { get; set; } = 5;
    public string? Collection { get; set; }

}
