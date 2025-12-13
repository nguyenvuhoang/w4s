using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
/// <typeparam name="InputValueType"></typeparam>
public class ActionsResponseModel<InputValueType> : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public ActionsResponseModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("fo")]
    public List<FoResponseModel<InputValueType>> fo { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>

    [JsonProperty("error")]
    public List<ErrorInfoModel> error { get; set; } = [];
}
