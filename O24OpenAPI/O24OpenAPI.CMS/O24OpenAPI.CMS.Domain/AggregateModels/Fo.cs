using Newtonsoft.Json;

namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class Fo : BaseEntity
{
    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("input")]
    public string? Input { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("actions")]
    public string? Actions { get; set; }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("request")]
    public string? Request { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("txtype")]
    public string? Txtype { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("txcode")]
    public string? Txcode { get; set; }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("version")]
    public int Version { get; set; } = 1;

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("updateTime")]
    public long Updatetime { get; set; } = DateTime.Now.Ticks;

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("txname")]
    public string? Txname { get; set; }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("isOld")]
    public bool Isold { get; set; } = false;

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("order")]
    public int DisplayOrder { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string? App { get; set; }
}
