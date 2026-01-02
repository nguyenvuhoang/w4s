using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.NCH.API.Application.Models.Response;

public class MailTemplateResponse : BaseO24OpenAPIModel
{
    public string TemplateId { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string DataSample { get; set; }
    public bool SendAsPDF { get; set; }
    public string Attachments { get; set; }
}
