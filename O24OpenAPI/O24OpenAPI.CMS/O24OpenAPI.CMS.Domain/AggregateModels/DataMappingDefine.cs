namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class DataMappingDefine : BaseEntity
{
    public string? ServiceId { get; set; }
    public string? WorkflowId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public bool Enable { get; set; }
    public bool TwoSide { get; set; }
}
