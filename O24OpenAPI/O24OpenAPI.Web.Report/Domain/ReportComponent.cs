using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain;

public class ReportComponent : BaseEntity
{
    public string ReportCode { get; set; }
    public string ComponentType { get; set; }
    public string ComponentName { get; set; }
    public string ClientRectangle { get; set; }
    public string Height { get; set; }
}
