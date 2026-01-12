using O24OpenAPI.Client.Enums;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Models.WorkflowStepModels;

public class WorkflowStepModel
{
    [JsonPropertyName("workflow_id")]
    public string WorkflowId { get; set; }

    [JsonPropertyName("step_order")]
    public int StepOrder { get; set; }

    [JsonPropertyName("step_code")]
    public string StepCode { get; set; }

    [JsonPropertyName("service_id")]
    public string ServiceId { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("sending_template")]
    public string SendingTemplate { get; set; } = "";

    [JsonPropertyName("sub_sending_template")]
    public string SubSendingTemplate { get; set; } = "";

    [JsonPropertyName("mapping_response")]
    public string MappingResponse { get; set; }

    [JsonPropertyName("step_timeout")]
    public long StepTimeout { get; set; } = 60000;

    [JsonPropertyName("sending_condition")]
    public string SendingCondition { get; set; } = "";

    [JsonPropertyName("processing_number")]
    public ProcessNumber ProcessingNumber { get; set; }

    [JsonPropertyName("is_reverse")]
    public bool IsReverse { get; set; } = false;

    [JsonPropertyName("should_await_step")]
    public bool ShouldAwaitStep { get; set; } = true;
}
