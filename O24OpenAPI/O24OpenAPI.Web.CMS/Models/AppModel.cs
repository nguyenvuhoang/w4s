#region Assembly Jits.Neptune.Web.Framework, Version=1.0.2.10, Culture=neutral, PublicKeyToken=null
// Jits.Neptune.Web.Framework.dll
#endregion


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class AppModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public AppModel()
    {
        this.ConfigTemplate = new JObject();
        this.ConnectOtherSystemStatus = "false";
    }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int Id { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("list_appliaction_id")]
    public string ListApplicationId { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("list_appliaction_name")]
    public string ListApplicationName { get; set; } = string.Empty;
    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("list_appliaction_des")]
    public string ListApplicationDes { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("list_appliaction_img")]
    public string ListApplicationImg { get; set; } = string.Empty;
    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("list_appliaction_bo")]
    public string ListApplicationBo { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("list_appliaction_order")]
    public string ListApplicationOrder { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("list_application_bo_logout_all")]
    public string ListApplicationBoLogoutAll { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("list_application_bo_logout")]
    public string ListApplicationBoLogout { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("ConnectOtherSystemStatus")]
    public string ConnectOtherSystemStatus { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("configTemplate")]
    public JObject ConfigTemplate { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string status { get; set; } = "A";




}
