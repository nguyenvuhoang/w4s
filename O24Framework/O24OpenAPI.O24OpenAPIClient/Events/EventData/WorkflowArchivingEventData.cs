namespace O24OpenAPI.O24OpenAPIClient.Events.EventData;

/// <summary>
/// The workflow archiving event data class
/// </summary>
public class WorkflowArchivingEventData
{
    /// <summary>
    /// Gets or sets the value of the workflow step execution list
    /// </summary>
    public List<WorkflowStepExecutionEventData> workflow_step_execution_list { get; set; } =
        new List<WorkflowStepExecutionEventData>();
}
