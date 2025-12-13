using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Services.Interfaces;

public interface IWorkflowStepService
{
    Task<WorkflowStep> GetByIdAsync(int id);

    Task<IList<WorkflowStep>> GetAllAsync();

    Task<WorkflowStep> AddAsync(WorkflowStep entity);
    Task UpdateAsync(WorkflowStep entity);
    Task DeleteAsync(WorkflowStep entity);
    Task DeleteByWorkflowIdAsync(string workflowId);
    Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model);
    Task<WorkflowStep> GetByWorkflowIdAndStepCode(string workflowId, string stepCode);
    Task<WorkflowStep> GetByWorkflowIdAndStepCodeAndStepOrder(string workflowId, string stepCode, int stepOrder);
    Task<IPagedList<WorkflowStep>> GetByWorkflowId(string workflowId);
}
