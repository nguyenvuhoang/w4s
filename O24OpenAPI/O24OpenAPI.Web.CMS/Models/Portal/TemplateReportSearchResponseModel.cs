using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Portal;

public class TemplateReportSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// TemplateReport id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("version")]
    public string Version { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string OrganizationId { get; set; }
}

/// <summary>
/// CodeListSearchResponseModel
/// </summary>
public partial class TemplateReportViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// TemplateReport id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("file_content")]
    public string FileContent { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("version")]
    public string Version { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string OrganizationId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; }
}
