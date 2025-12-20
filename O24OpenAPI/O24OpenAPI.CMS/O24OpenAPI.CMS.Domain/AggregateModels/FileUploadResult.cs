namespace O24OpenAPI.CMS.Domain.AggregateModels;

public class FileUploadResult : BaseEntity
{
    public string Key { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Status { get; set; } = default!;
}
