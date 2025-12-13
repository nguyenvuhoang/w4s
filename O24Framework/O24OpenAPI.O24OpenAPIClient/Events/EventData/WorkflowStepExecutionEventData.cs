namespace O24OpenAPI.O24OpenAPIClient.Events.EventData;

/// <summary>
/// The workflow step execution event data class
/// </summary>
public class WorkflowStepExecutionEventData
{
    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the step execution id
    /// </summary>
    public string step_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    public string service_id { get; set; }
}
