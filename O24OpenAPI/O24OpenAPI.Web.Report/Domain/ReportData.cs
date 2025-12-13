using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain;

public class ReportData : BaseEntity
{
    public string ReportCode { get; set; }
    public string DataSourceName { get; set; }
    public string DataSource { get; set; }
    public string DataBand { get; set; }
    public string ParentDatatable { get; set; }
}
