using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Lib;

namespace O24OpenAPI.WFO.Models;

public enum Enum_STATUS
{
    NotProcessed,
    Created,
    Completed,
    Error,
    Skipped,
}

public class WorkflowExecution
{
    public WorkflowInfoModel execution { get; set; } = new();

    public List<WorkflowStepInfoModel> execution_steps { get; set; } = new();

    public string error_message { get; set; }
}

public class WorkflowInfoModel : BaseO24OpenAPIModel
{
    public WorkflowInfoModel() { }

    public WorkflowInfoModel(WorkflowInput workflowInput)
    {
        execution_id = workflowInput.ExecutionId;
        workflow_id = workflowInput.WorkflowId;
        status = Enum_STATUS.Created;
        input = workflowInput;
        created_on = Common.GetCurrentDateAsLongNumber();
        transaction_date = workflowInput.TransactionDate.ToString();
        value_date = workflowInput.ValueDate.ToString();
    }

    public string execution_id { get; set; }

    public string correlation_id { get; set; }

    public object input { get; set; }

    public string input_string { get; set; }

    public string workflow_id { get; set; }

    public Enum_STATUS status { get; set; }
    public string error { get; set; }

    public long created_on { get; set; }

    public long finish_on { get; set; }

    public string is_timeout { get; set; }

    public string is_processing { get; set; }

    public string is_success { get; set; }

    public string workflow_type { get; set; }
    public object response_content { get; set; }

    public string reversed_execution_id { get; set; }

    public string reversed_by_execution_id { get; set; }

    public string is_disputed { get; set; }

    public long archiving_time { get; set; }

    public long purging_time { get; set; }

    public string approved_execution_id { get; set; }

    public string transaction_number { get; set; }

    public string transaction_date { get; set; }

    public string value_date { get; set; }

    public long GetExecutionTime()
    {
        return finish_on - created_on;
    }
}

public class WorkflowStepInfoModel : BaseO24OpenAPIModel
{
    public WorkflowStepInfoModel() { }

    public WorkflowStepInfoModel(WFScheme wfScheme)
    {
        step_execution_id = wfScheme.request.request_header.step_execution_id;
        execution_id = wfScheme.request.request_header.execution_id;
        step_order = wfScheme.request.request_header.step_order;
        step_code = wfScheme.request.request_header.step_code;
        p1_start = Common.GetCurrentDateAsLongNumber();
        p1_status = Enum_STATUS.Created;
        p1_request = wfScheme;
        p2_status = Enum_STATUS.NotProcessed;
    }

    public bool should_await { get; set; } = true;
    public string step_execution_id { get; set; }
    public string execution_id { get; set; }
    public int step_order { get; set; }
    public string step_code { get; set; }
    public object sending_condition { get; set; }
    public object p1_request { get; set; }
    public long p1_start { get; set; }
    public long p1_finish { get; set; }
    public Enum_STATUS p1_status { get; set; }
    public string p1_error { get; set; }
    public object p1_content { get; set; }
    public object p2_request { get; set; }
    public long p2_start { get; set; }
    public long p2_finish { get; set; }
    public Enum_STATUS p2_status { get; set; }
    public string p2_error { get; set; }
    public string p2_error_code { get; set; }
    public object p2_content { get; set; }
    public string is_success { get; set; }
    public string is_timeout { get; set; }
    public string execution_servcie { get; set; }
}
