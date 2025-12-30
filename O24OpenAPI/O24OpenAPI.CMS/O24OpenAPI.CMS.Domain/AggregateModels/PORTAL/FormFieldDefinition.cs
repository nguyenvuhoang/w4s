namespace O24OpenAPI.CMS.Domain.AggregateModels.PORTAL;

public partial class FormFieldDefinition : BaseEntity
{
    public string FormId { get; set; }
    public string FieldName { get; set; }
    public string FieldValue { get; set; } = string.Empty;
}

