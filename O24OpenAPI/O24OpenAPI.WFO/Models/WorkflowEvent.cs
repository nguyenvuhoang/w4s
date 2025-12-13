using O24OpenAPI.O24OpenAPIClient.Events;

namespace O24OpenAPI.WFO.Models;

public class WorkflowEvent
{
    public O24OpenAPIWorkflowEventTypeEnum EventType { get; set; }
    public string ServiceHandleEvent { get; set; }
}
