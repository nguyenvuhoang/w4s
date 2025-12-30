using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

public partial class ReportConfig : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public ReportConfig() { }

    /// <summary>
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("code_template")]
    public string CodeTemplate { get; set; }

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
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("data_source")]
    public string DataSource { get; set; }

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
    [JsonProperty("full_class_name")]
    public string FullClassName { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("method_name")]
    public string MethodName { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("is_async")]
    public bool IsAsync { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("mail_config_code")]
    public string MailConfigCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string OrganizationId { get; set; }
}
