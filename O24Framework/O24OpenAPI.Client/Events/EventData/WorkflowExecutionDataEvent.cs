using O24OpenAPI.Client.Workflow;

namespace O24OpenAPI.Client.Events.EventData;

/// <summary>
/// The workflow execution data event class
/// </summary>
public class WorkflowExecutionDataEvent
{
    /// <summary>
    /// Gets or sets the value of the execution
    /// </summary>
    public WorkflowExecutionInfor? execution { get; set; }

    /// <summary>
    /// Gets or sets the value of the execution steps
    /// </summary>
    public List<WorkflowStepInfor>? execution_steps { get; set; }

    /// <summary>
    /// Gets or sets the value of the error message
    /// </summary>
    public string? error_message { get; set; }
}
