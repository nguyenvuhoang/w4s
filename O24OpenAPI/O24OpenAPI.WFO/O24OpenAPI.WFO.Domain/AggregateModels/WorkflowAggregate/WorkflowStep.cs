using Newtonsoft.Json;
using O24OpenAPI.Client.Enums;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public partial class WorkflowStep : BaseEntity
{
    [JsonProperty("workflow_id")]
    public string? WorkflowId { get; set; }

    [JsonProperty("step_order")]
    public int StepOrder { get; set; }

    [JsonProperty("step_code")]
    public string? StepCode { get; set; }

    [JsonProperty("service_id")]
    public string? ServiceId { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; } = true;

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("sending_template")]
    public string SendingTemplate { get; set; } = "";

    [JsonProperty("sub_sending_template")]
    public string SubSendingTemplate { get; set; } = "";

    [JsonProperty("mapping_response")]
    public string? MappingResponse { get; set; }

    [JsonProperty("step_timeout")]
    public long StepTimeout { get; set; } = 60000;

    [JsonProperty("sending_condition")]
    public string SendingCondition { get; set; } = "";

    [JsonProperty("processing_number")]
    public ProcessNumber ProcessingNumber { get; set; }

    [JsonProperty("is_reverse")]
    public bool IsReverse { get; set; } = false;

    [JsonProperty("should_await_step")]
    public bool ShouldAwaitStep { get; set; } = true;
}
