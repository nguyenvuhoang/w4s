namespace O24OpenAPI.CMS.API.Application.Models.Template;

public class TemplateModel : BaseO24OpenAPIModel
{
    public string App { get; set; }
    public string TemplateId { get; set; }
    public string Name { get; set; }
    public List<Dictionary<string, object>> Layout { get; set; }
    public string Rules { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
