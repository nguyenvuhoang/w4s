using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IWorkflowStepService
{
    Task<WorkflowStep> GetByWorkflowIdAndStep(string workflowId, string stepCode);
    Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model);

    Task<IPagedList<WorkflowStep>> AdvancedSearch(WorkflowStepSearchModel model);

    Task<List<WorkflowStep>> GetStepsByWorkflowIdAsync(string workflowId, string appCode);
}
