using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Models;

public class WorkflowExecutionContext
{
    public string ExecutionId { get; set; }
    public int CurrentStepIndex { get; set; }
    public List<StepExecutionContext> StepContexts { get; set; }
    public WorkflowInput Input { get; set; }
    public bool IsReverseFlow { get; set; }
    public List<WorkflowEvent> WorkflowEvents { get; set; } = [];
    public Dictionary<string, object> ContextData { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);

    public WorkflowExecutionContext() { }

    public WorkflowExecutionContext(WorkflowDef workflowDef, List<WorkflowStep> workflowSteps)
    {
        StepContexts = [];
        foreach (var step in workflowSteps)
        {
            StepContexts.Add(new StepExecutionContext() { WorkflowStep = step });
        }
        if (!string.IsNullOrWhiteSpace(workflowDef.WorkflowEvent))
        {
            WorkflowEvents = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkflowEvent>>(
                workflowDef.WorkflowEvent
            );
        }
    }

    public class StepExecutionContext
    {
        public WorkflowStep WorkflowStep { get; set; }
        public WFScheme WFScheme { get; set; }
    }
}
