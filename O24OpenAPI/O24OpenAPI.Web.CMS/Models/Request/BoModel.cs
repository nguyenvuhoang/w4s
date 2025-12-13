#region Assembly Jits.Neptune.Web.Framework, Version=1.0.2.10, Culture=neutral, PublicKeyToken=null
// Jits.Neptune.Web.Framework.dll
#endregion


using Newtonsoft.Json;
using O24OpenAPI.Web.CMS.Models;

namespace Jits.Neptune.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class BoModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public BoModel() { }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int Id { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("input")]
    public Dictionary<string, object> Input { get; set; } = new Dictionary<string, object>();

    // public Dictionary<string, object> InputDictionary
    // {
    //     get => JsonConvert.DeserializeObject<Dictionary<string, object>>(Input);
    //     set => Input = JsonConvert.SerializeObject(value);
    // }
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("actions")]
    public List<BoAction> Actions { get; set; } = new List<BoAction>();

    // public List<BoAction> ActionsDictionary
    // {
    //     get => JsonConvert.DeserializeObject<List<BoAction>>(Actions);
    //     set => Actions = JsonConvert.SerializeObject(value);
    // }
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("response")]
    public ActionsResponseModel<object> Response { get; set; } = new ActionsResponseModel<object>();

    // public ActionsResponseModel<object> ResponseDictionary
    // {
    //     get => JsonConvert.DeserializeObject<ActionsResponseModel<object>>(Response);
    //     set => Response = JsonConvert.SerializeObject(value);
    // }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("txtype")]
    public string Txtype { get; set; } = string.Empty;
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
    public int Version { get; set; } = 0;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; } = 1582818949391;
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
public partial class BoAction : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public BoAction() { }
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
    [JsonProperty("function")]
    public string Function { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("para")]
    public List<object> Para { get; set; } = null;
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [JsonProperty("response")]
    public Dictionary<string, ActionsResponseModel<object>> Response { get; set; } = new Dictionary<string, ActionsResponseModel<object>>();

}
