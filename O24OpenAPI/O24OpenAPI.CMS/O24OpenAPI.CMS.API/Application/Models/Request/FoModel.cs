using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class FoModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public FoModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("input")]
    public Dictionary<string, object> Input { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("actions")]
    public List<Dictionary<string, object>> Actions { get; set; } =
        [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("request")]
    public Dictionary<string, object> Request { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; } = "A";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txtype")]
    public string Txtype { get; set; } = "fo";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txcode")]
    public string Txcode { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("version")]
    public int Version { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; } = new DateTime().Ticks;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txname")]
    public string Txname { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("hasRole")]
    public string HasRole { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("isOld")]
    public bool IsOld { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("order")]
    public int DisplayOrder { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; } = string.Empty;
}

/// <summary>
///
/// </summary>
public class FoCreateModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public FoCreateModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("input")]
    public Dictionary<string, object> Input { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("actions")]
    public List<Dictionary<string, object>> Actions { get; set; } =
        [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("request")]
    public Dictionary<string, object> Request { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; } = "A";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txtype")]
    public string Txtype { get; set; } = "fo";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txcode")]
    public string Txcode { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("version")]
    public int Version { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; } = new DateTime().Ticks;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txname")]
    public string Txname { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("hasRole")]
    public string HasRole { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("isOld")]
    public bool IsOld { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("order")]
    public int Order { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; } = string.Empty;
}