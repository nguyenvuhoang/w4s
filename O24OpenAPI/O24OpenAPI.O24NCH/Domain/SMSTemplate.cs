using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class SMSTemplate : BaseEntity
{
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public SMSTemplate Clone()
    {
        return (SMSTemplate)this.MemberwiseClone();
    }
}
