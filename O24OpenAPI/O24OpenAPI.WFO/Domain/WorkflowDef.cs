using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.WFO.Constant.Status;

namespace O24OpenAPI.WFO.Domain;

public partial class WorkflowDef : BaseEntity
{
    [JsonProperty("workflow_id")]
    public string WorkflowId { get; set; }

    [JsonProperty("workflow_name")]
    public string WorkflowName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("channel_id")]
    public string ChannelId { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; } = WorkflowStatus.Active;

    [JsonProperty("timeout")]
    public long Timeout { get; set; } = 60000;

    [JsonProperty("is_reverse")]
    public bool IsReverse { get; set; } = false;

    [JsonProperty("template_response")]
    public string TemplateResponse { get; set; }

    [JsonProperty("workflow_event")]
    public string WorkflowEvent { get; set; }

    [JsonProperty("created_on_utc")]
    public DateTime? CreatedOnUtc { get; set; }

    [JsonProperty("updated_on_utc")]
    public DateTime? UpdatedOnUtc { get; set; }
}
