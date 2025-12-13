using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.WFO.Domain;

public class WorkflowInfo : BaseEntity
{
    public WorkflowInfo() { }

    public string execution_id { get; set; }

    public string correlation_id { get; set; }

    public string input { get; set; }

    public string input_string { get; set; }

    public string workflow_id { get; set; }

    public int status { get; set; }

    public string error { get; set; }

    public long created_on { get; set; }

    public long finish_on { get; set; }

    public string is_timeout { get; set; }

    public string is_processing { get; set; }

    public string is_success { get; set; }

    public string workflow_type { get; set; }
    public string response_content { get; set; }

    public string reversed_execution_id { get; set; }

    public string reversed_by_execution_id { get; set; }

    public string is_disputed { get; set; }

    public long archiving_time { get; set; }

    public long purging_time { get; set; }

    public string approved_execution_id { get; set; }

    public string transaction_number { get; set; }

    public string transaction_date { get; set; }

    public string value_date { get; set; }
}
