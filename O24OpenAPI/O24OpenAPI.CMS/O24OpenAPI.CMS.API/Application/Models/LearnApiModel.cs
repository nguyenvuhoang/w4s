using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Models;

public class LearnApiModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public LearnApiModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_id")]
    public string LearnApiId { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_name")]
    public string LearnApiName { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_data")]
    public string LearnApiData { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_node_data")]
    public string LearnApiNodeData { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_method")]
    public string LearnApiMethod { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_header")]
    public string LearnApiHeader { get; set; }

    // /// <summary>
    // ///
    // /// </summary>
    [JsonProperty("full_interface_name")]
    public string FullInterfaceName { get; set; }

    // /// <summary>
    // ///
    // /// </summary>
    [JsonProperty("method_name")]
    public string MethodName { get; set; } = "{}";

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_mapping")]
    public string LearnApiMapping { get; set; } = "{}";

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("learn_api_id_clear")]
    public string LearnApiIdClear { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("key_read_data")]
    public string KeyReadData { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("is_cache")]
    public bool IsCache { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("learn_api_mapping_response")]
    public string LearnApiMappingResponse { get; set; }
}

public class LearnApiRequestModel
{
    public string Lang { get; set; } = string.Empty;

    public Dictionary<string, object> fields = [];

    public string LearnApiId = string.Empty;
    public int RequestId { get; set; }

    [JsonProperty("workflow_execution_id")]
    public string ExecuteId { get; set; }

    [JsonProperty("workflow_func")]
    public string WorkflowFunc { get; set; }

    [JsonProperty("table_name")]
    public string TableName { get; set; }

    [JsonProperty("id_field_name")]
    public string IdFieldName { get; set; }

    [JsonProperty("action_type")]
    public string ActionType { get; set; }

    [JsonProperty("module")]
    public string Module { set; get; } = "";

    [JsonProperty("tx_code")]
    public string TxCode { set; get; } = "";

    [JsonProperty("tx_ref_id")]
    public string TxRefId { set; get; } = "";

    public string LearnApiMappingResponse { get; set; }

    public JObject ObjectField { get; set; }

    public string StringWorkingDate { get; set; }

    public string LearnApiIdClear { get; set; }

    public bool IsCache { get; set; }

    public DateTime? ValueDate { get; set; }

    public bool IsWorkflow { get; set; }

    public bool IsFrontOfficeMapping { get; set; } = true;

    [JsonProperty("config_mapping_response")]
    public string ConfigMappingResponse { get; set; } = "A";

    public int WorkflowProcessingNumber { get; set; }
    public string WorkflowId { get; set; }
    public string ChannelId { get; set; }
}
