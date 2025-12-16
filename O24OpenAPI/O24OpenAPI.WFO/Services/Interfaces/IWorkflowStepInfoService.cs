using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Services.Interfaces;

public interface IWorkflowStepInfoService
{
    Task AddAsync(WorkflowStepInfo workflowStepInfo);
    Task BulkAddAsync(List<WorkflowStepInfo> workflowStepInfos);
    Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme);
    Task<WorkflowStepInfo> GetByExecutionStep(string executionId, string stepExecutionId);
}
