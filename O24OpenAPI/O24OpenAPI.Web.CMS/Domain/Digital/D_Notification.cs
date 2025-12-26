namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_NOTIFICATION : BaseEntity
{
    public string UserCode { get; set; }
    public string AppType { get; set; }
    public string NotificationType { get; set; }
    public string DataValue { get; set; }
    public string TemplateID { get; set; }
    public string Redirect { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsPushed { get; set; } = false;
    public DateTime DateTime { get; set; }
    public bool IsProcessed { get; set; } = false;
}
