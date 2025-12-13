using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.MIS;

public class MISIndicatorDefinition : BaseEntity
{
    public string ReportCode { get; set; }
    public string ColumnName { get; set; }
    public string Condition { get; set; }
    public string Formula { get; set; }
    public string DataSource { get; set; }
    public string Group { get; set; }
}
