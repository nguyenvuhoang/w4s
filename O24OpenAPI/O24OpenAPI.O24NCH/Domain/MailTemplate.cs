using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class MailTemplate : BaseEntity
{
    public string TemplateId { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string DataSample { get; set; }
    public bool SendAsPDF { get; set; }
    public string Attachments { get; set; }

    public MailTemplate() { }
    public MailTemplate(string templateId, string status, string description, string subject, string body, string dataSample, bool sendAsPDF, string attachments)
    {
        TemplateId = templateId;
        Status = status;
        Description = description;
        Subject = subject;
        Body = body;
        DataSample = dataSample;
        SendAsPDF = sendAsPDF;
        Attachments = attachments;
    }
}
