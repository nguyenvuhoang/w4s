using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class ConditionOrder
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("type")]
    public string Type{ get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_then_by")]
    public bool IsThenBy { get; set; } = false;
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_null_first")]
    public bool IsNullFirst {get;set;} = false;
}