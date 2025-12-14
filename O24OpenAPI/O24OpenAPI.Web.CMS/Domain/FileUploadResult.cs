namespace O24OpenAPI.Web.CMS.Domain;

public class FileUploadResult : BaseEntity
{
    public string Key { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Status { get; set; } = default!;
}
