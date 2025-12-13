using O24OpenAPI.WFO.Models;

namespace O24OpenAPI.WFO.Services;

public interface IWorkflowExecutionService
{
    Task<O24OpenAPIClient.Workflow.WorkflowResponse> StartWorkflowAsync(WorkflowInput input);
}
