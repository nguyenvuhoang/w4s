using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Domain;

public partial class LearnApi : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public LearnApi() { }

    /// <summary>
    /// User code
    /// </summary>
    [JsonProperty("learn_api_id")]
    public string LearnApiId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_name")]
    public string LearnApiName { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_data")]
    public string LearnApiData { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_node_data")]
    public string LearnApiNodeData { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_method")]
    public string LearnApiMethod { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_header")]
    public string LearnApiHeader { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_mapping")]
    public string LearnApiMapping { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool IsCache { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("key_read_data")]
    public string KeyReadData { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string LearnApiIdClear { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("app")]
    public string App { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_mapping_response")]
    public string LearnApiMappingResponse { get; set; }

    // /// <summary>
    // ///
    // /// </summary>
    [JsonProperty("full_interface_name")]
    public string FullInterfaceName { get; set; }

    // /// <summary>
    // ///
    // /// </summary>
    [JsonProperty("method_name")]
    public string MethodName { get; set; }

    [JsonProperty("uri")]
    public string URI { get; set; } = string.Empty;
}
