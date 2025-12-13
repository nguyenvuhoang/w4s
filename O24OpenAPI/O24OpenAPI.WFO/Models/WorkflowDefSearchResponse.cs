using Newtonsoft.Json;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.WFO.Models;

public class WorkflowDefSearchResponse : BaseO24OpenAPIModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("workflow_id")]
    public string WorkflowId { get; set; }

    [JsonProperty("workflow_name")]
    public string WorkflowName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("channel_id")]
    public string ChannelId { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("timeout")]
    public long Timeout { get; set; }

    [JsonProperty("is_reverse")]
    public bool IsReverse { get; set; }

    [JsonProperty("template_response")]
    public string TemplateResponse { get; set; }
}
