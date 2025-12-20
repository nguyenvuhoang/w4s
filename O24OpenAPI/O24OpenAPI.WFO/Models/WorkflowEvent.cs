using O24OpenAPI.Client.Events;

namespace O24OpenAPI.WFO.Models;

public class WorkflowEvent
{
    public O24OpenAPIWorkflowEventTypeEnum EventType { get; set; }
    public string ServiceHandleEvent { get; set; }
}
