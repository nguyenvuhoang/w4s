using System.Text.Json.Serialization;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
/// The bo request model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class BoRequestModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public BoRequestModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonPropertyName("bo")]
    public List<BoRequest> Bo { get; set; } = [];
}

/// <summary>
///
/// </summary>
public class BoRequest : BaseO24OpenAPIModel
{
    [JsonPropertyName("use_microservice")]
    public bool UseMicroservice { get; set; } = false;

    [JsonPropertyName("input")]
    public Dictionary<string, object> Input { get; set; } = [];
}
