using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

public partial class ReportTemplate : BaseEntity
{
    /// <summary>
    /// code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// version
    /// </summary>
    [JsonProperty("version")]
    public string Version { get; set; }

    /// <summary>
    /// app
    /// </summary>
    [JsonProperty("app")]
    public string App { get; set; }

    /// <summary>
    /// status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    /// Orientation (Portrait, Landscape)
    /// </summary>
    [JsonProperty("orientation")]
    public string Orientation { get; set; }

    /// <summary>
    /// page_width
    /// </summary>
    [JsonProperty("page_width")]
    public string PageWidth { get; set; }

    /// <summary>
    /// page_height
    /// </summary>
    [JsonProperty("page_height")]
    public string PageHeight { get; set; }

    /// <summary>
    /// paper_size (A4, Letter, Legal)
    /// </summary>
    [JsonProperty("paper_size")]
    public string PaperSize { get; set; }

    /// <summary>
    /// Watermark
    /// </summary>
    [JsonProperty("watermark")]
    public string Watermark { get; set; }

    /// <summary>
    /// border
    /// </summary>
    [JsonProperty("border")]
    public string Border { get; set; }

    /// <summary>
    /// margins
    /// </summary>
    [JsonProperty("margins")]
    public string Margins { get; set; }

    /// <summary>
    /// reportunit
    /// </summary>
    [JsonProperty("reportunit")]
    public string ReportUnit { get; set; }

    /// <summary>
    /// file_content
    /// </summary>
    [JsonProperty("file_content")]
    public string FileContent { get; set; }

    /// <summary>
    /// file_content
    /// </summary>
    [JsonProperty("font")]
    public string Font { get; set; }

    /// <summary>
    /// description
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// organization_id
    /// </summary>
    [JsonProperty("organization_id")]
    public string OrganizationId { get; set; }

    /// <summary>
    /// ReportTemplate
    /// </summary>
    public ReportTemplate() { }
}
