namespace O24OpenAPI.CMS.Domain.AggregateModels;

public class FormFieldDefinition : BaseEntity
{
    public string FormId { get; set; }
    public string FieldName { get; set; }
    public string FieldValue { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; set; }
}
