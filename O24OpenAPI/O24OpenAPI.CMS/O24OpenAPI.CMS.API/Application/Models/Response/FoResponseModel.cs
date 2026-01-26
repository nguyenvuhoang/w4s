using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models.Response;

/// <summary>
///
/// </summary>
/// <typeparam name="InputValueType"></typeparam>
public class FoResponseModel<InputValueType> : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public FoResponseModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txcode")]
    public string txcode { get; set; } = string.Empty;

    // [JsonProperty("executeId")]
    public string executeId { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("input")]
    public Dictionary<string, InputValueType> input { get; set; } =
        new Dictionary<string, InputValueType>();
}
