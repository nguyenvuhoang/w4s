using Newtonsoft.Json;

namespace O24OpenAPI.CMS.Domain.AggregateModels.PORTAL;

/// <summary>
/// Represents an Bo
/// </summary>
public partial class Form : BaseEntity
{
    /// <summary>
    /// info
    /// </summary>
    [JsonProperty("info")]
    public string? Info { get; set; }

    /// <summary>
    /// list_layout
    /// </summary>
    [JsonProperty("list_layout")]
    public string? ListLayout { get; set; }

    /// <summary>
    /// form_id
    /// </summary>
    [JsonProperty("form_id")]
    public string? FormId { get; set; }

    /// <summary>
    /// app
    /// </summary>
    [JsonProperty("app")]
    public string? App { get; set; }

    public string? MasterData { get; set; }
}
