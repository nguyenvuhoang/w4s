using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class SMSProvider : BaseEntity
{
    public string ProviderName { get; set; } = string.Empty;
    // "http://unicontact.unitel.com.la:8181/apiSendSms.php?wsdl"
    public string ApiUrl { get; set; } = string.Empty;
    public string CountryPrefix { get; set; } = string.Empty;
    public string AllowedPrefix { get; set; } = string.Empty;
    public string ApiUsername { get; set; } = string.Empty;
    public string ApiPassword { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public SMSProvider Clone()
    {
        return (SMSProvider)this.MemberwiseClone();
    }
}
