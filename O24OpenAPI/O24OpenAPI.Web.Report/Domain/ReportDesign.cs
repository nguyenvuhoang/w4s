using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain;

public class ReportDesign : BaseEntity
{
    public ReportDesign() { }

    public string ReportCode { get; set; }
    public string ComponentParent { get; set; }
    public string ComponentType { get; set; }
    public string ComponentName { get; set; }
    public string Text { get; set; }
    public string Image { get; set; }
    public string FontFamily { get; set; }
    public int? FontSize { get; set; }
    public string FontStyle { get; set; }
    public string TextAlign { get; set; }
    public string HorAlignment { get; set; }
    public string VertAlignment { get; set; }
    public string TextBrush { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string PositionX { get; set; }
    public string PositionY { get; set; }
    public string BorderColor { get; set; }
    public string BorderThickness { get; set; }
    public string BackgroundColor { get; set; }
    public bool GrowToHeight { get; set; }
    public int BreakOfPage { get; set; }
    public string BandType { get; set; }
    public string BandName { get; set; }
    public string DataSource { get; set; }
    public string FieldName { get; set; }
    public string TargetValue { get; set; }
    public string Mask { get; set; }
    public int OrderBy { get; set; }
}
