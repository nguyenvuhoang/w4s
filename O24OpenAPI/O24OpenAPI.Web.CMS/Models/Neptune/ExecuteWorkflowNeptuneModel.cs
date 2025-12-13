using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace O24OpenAPI.Web.CMS.Models.Neptune;

public class ExecuteWorkflowNeptuneModel : BaseTransactionModel
{
    [JsonPropertyName("fields")]
    public Dictionary<string, object> Fields { get; set; }

    [JsonPropertyName("workflowid")]
    [JsonProperty("workflowid")]
    public string WorkflowId { get; set; }
}
