using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public partial class EmailSendOut : BaseEntity
{
    public string ConfigId { get; set; }
    public string TemplateId { get; set; }
    public string Receiver { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Attachments { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
