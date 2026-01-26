using System.Text.Json.Serialization;

namespace O24OpenAPI.CMS.API.Application.Models;

public class ParaServerModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int Id { get; set; }

    /// <summary>
    /// ///
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("datatype")]
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("app")]
    public string App { get; set; } = string.Empty;
}
