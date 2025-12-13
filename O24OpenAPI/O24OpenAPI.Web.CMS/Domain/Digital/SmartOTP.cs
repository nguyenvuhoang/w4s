namespace O24OpenAPI.Web.CMS.Domain;

public class SmartOTPInfo : BaseEntity
{
    public string UserCode { get; set; }
    public string PrivateKey { get; set; }
    public string DeviceId { get; set; }
    public string PinCode { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.UtcNow;
}
