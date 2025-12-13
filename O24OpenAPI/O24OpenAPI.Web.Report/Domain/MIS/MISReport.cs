using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public class MISReport : BaseEntity
{
    public string ReportCode { get; set; }
    public string Item { get; set; }
    public int Order { get; set; }
    public string ValueBasis { get; set; }
    public string Condition { get; set; }
    public string DataSource { get; set; }
    public string Group { get; set; }
}
