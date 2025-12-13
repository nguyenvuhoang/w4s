#region Assembly Jits.Neptune.Web.Framework, Version=1.0.2.10, Culture=neutral, PublicKeyToken=null
// Jits.Neptune.Web.Framework.dll
#endregion


using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class ConfigOfDomainModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public ConfigOfDomainModel() { }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("rsa")]
    public string rsa { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("template_host_dev")]
    public string template_host_dev { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("server_version")]
    public string server_version { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("server_name")]
    public string server_name { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("isDev")]
    public bool isDev { get; set; } = true;


    // [JsonProperty("gatewayInfo")]
    // public Dictionary<string, AppGatewaySetting> gatewayInfo { get; set; }
}
