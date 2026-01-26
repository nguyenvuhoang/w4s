using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public partial class WorkflowStepInfo : BaseEntity
{
    public WorkflowStepInfo() { }

    public string step_execution_id { get; set; }
    public string execution_id { get; set; }
    public int step_order { get; set; }
    public string step_code { get; set; }
    public string sending_condition { get; set; }
    public string p1_request { get; set; }
    public long p1_start { get; set; }
    public long p1_finish { get; set; }
    public int p1_status { get; set; }
    public string p1_error { get; set; }
    public string p1_content { get; set; }
    public string p2_request { get; set; }
    public long p2_start { get; set; }
    public long p2_finish { get; set; }
    public int p2_status { get; set; }
    public string p2_error { get; set; }
    public string p2_error_code { get; set; }
    public string p2_content { get; set; }
    public string is_success { get; set; }
    public string is_timeout { get; set; }
    public string execution_servcie { get; set; }
}
