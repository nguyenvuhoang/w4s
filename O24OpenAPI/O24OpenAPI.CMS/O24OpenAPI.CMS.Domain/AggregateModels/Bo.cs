using Newtonsoft.Json;

namespace O24OpenAPI.CMS.Domain;

/// <summary>
/// The bo class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class Bo : BaseEntity
{
    /// <summary>
    /// ///
    /// </summary>
    public Bo() { }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("input")]
    public string Input { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("actions")]
    public string Actions { get; set; }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("response")]
    public string Response { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = "A";

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("txtype")]
    public string Txtype { get; set; } = "bo";

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("txcode")]
    public string Txcode { get; set; }

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
    public string Txname { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("hasRole")]
    public string Hasrole { get; set; } = "false";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("isOld")]
    public bool Isold { get; set; } = false;

    /// <summary>
    /// DisplayOrder
    /// </summary>
    [JsonProperty("order")]
    public int DisplayOrder { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; }
}
