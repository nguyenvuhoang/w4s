using O24OpenAPI.Client.Workflow;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;

namespace O24OpenAPI.WFO.Services;

public interface IWorkflowService
{
    Task<WorkflowDef> GetWorkflowDef(string workflowId, string ChannelId);
    Task<List<WorkflowStep>> GetWorkflowSteps(string workflowId);
    Task<(WorkflowDef, List<WorkflowStep>)> GetExecutingWorkflow(
        string workflowId,
        string channelId
    );
    Task<long> GetMaxTimeOut();
    Task<WorkflowResponse> CreateWorkflow(WorkflowInput input);
    Task<WorkflowResponse> SimpleSearch(WorkflowInput input);
    Task<WorkflowResponse> DeleteWorkflow(WorkflowInput input);
    Task<WorkflowResponse> UpdateWorkflow(WorkflowInput input);
}
