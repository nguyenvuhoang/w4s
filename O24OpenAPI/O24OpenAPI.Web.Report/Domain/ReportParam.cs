using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain;

public partial class ReportParam : BaseEntity
{
    public ReportParam() { }

    public string ReportCode { get; set; }

    public string ParamName { get; set; }

    public string ControlName { get; set; }

    public string ControlType { get; set; }

    public int? Width { get; set; }
    public int? Height { get; set; }
    public string DataStoreType { get; set; }
    public string Store { get; set; }
    public string View { get; set; }
    public string Key { get; set; }
    public string Text { get; set; }

    public string Value { get; set; }

    public string LangId { get; set; }

    public int? Orderby { get; set; }

    public string Tag { get; set; }

    public string TimeReport { get; set; }
}
