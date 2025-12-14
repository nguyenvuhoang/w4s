namespace O24OpenAPI.AI.Domain.AggregatesModel.AskAggreate
{
    public sealed class AskRequest
    {
        public string TenantId { get; set; } = default!;
        public string Question { get; set; } = default!;

        public string? DocType { get; set; }
        public string? Language { get; set; }

        public int TopK { get; set; } = 5;
        public float MinScore { get; set; } = 0.70f;
        public string? Collection { get; set; }
    }

}
