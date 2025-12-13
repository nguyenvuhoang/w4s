using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Services.Interfaces;

public interface IWorkflowInfoService
{
    Task AddAsync(WorkflowInfo workflowInfo);
}
