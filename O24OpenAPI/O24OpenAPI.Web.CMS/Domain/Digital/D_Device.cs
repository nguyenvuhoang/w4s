namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_DEVICE : BaseEntity
{
    public string UserCode { get; set; }
    public string DeviceId { get; set; }
    public string DeviceType { get; set; }
    public string AppType { get; set; }
    public string Status { get; set; } = DeviceStatus.ACTIVE;
    public string PushID { get; set; }
}
