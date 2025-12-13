using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Migrations;

public class WFOConditionField
{
    public static readonly List<string> WorkflowDefCondition =
    [
        nameof(WorkflowDef.WorkflowId),
        nameof(WorkflowDef.ChannelId),
    ];

    public static readonly List<string> WorkflowStepCondition =
    [
        nameof(WorkflowStep.WorkflowId),
        nameof(WorkflowStep.StepCode),
        nameof(WorkflowStep.StepOrder),
        nameof(WorkflowStep.ServiceId),
        nameof(WorkflowStep.Status)
    ];
}
