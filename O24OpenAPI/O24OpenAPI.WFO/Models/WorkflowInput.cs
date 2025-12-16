using Newtonsoft.Json;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Attributes;
using O24OpenAPI.WFO.Enum;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.Models;

public class WorkflowInput
{
    [JsonProperty("workflowid")]
    [JsonPropertyName("workflowid")]
    public string WorkflowId { get; set; }

    [JsonProperty("execution_id")]
    [JsonPropertyName("execution_id")]
    public string ExecutionId { get; set; }

    [JsonProperty("workflow_type")]
    [JsonPropertyName("workflow_type")]
    public EnumWorkflowType? WorkflowType { get; set; } = EnumWorkflowType.normal;

    [EnumSchemeValidation(typeof(EnumSupportLanguages))]
    [JsonPropertyName("lang")]
    [JsonProperty("lang")]
    public string Lang { get; set; } = EnumSupportLanguages.en.ToString();

    [JsonProperty("fields")]
    [JsonPropertyName("fields")]
    public Dictionary<string, object> Fields { get; set; }

    [JsonProperty("token")]
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    [JsonProperty("service_instances")]
    [JsonPropertyName("service_instances")]
    public InputServiceInstance[] ServiceInstances { get; set; } = [];

    [JsonProperty("user_id")]
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonProperty("channel_id")]
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    [JsonProperty("device")]
    [JsonPropertyName("device")]
    public DeviceModel Device { get; set; }

    [JsonProperty("transaction_date")]
    [JsonPropertyName("transaction_date")]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [JsonProperty("value_date")]
    [JsonPropertyName("value_date")]
    public DateTime ValueDate { get; set; } = DateTime.Now;
}
