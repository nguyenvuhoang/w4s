using O24OpenAPI.Core;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public interface IWorkflowStepRepository : IRepository<WorkflowStep>
{
    Task<List<WorkflowStep>> GetByWorkflowIdAsync(string workflowId);
    Task DeleteByWorkflowIdAsync(string workflowId);
    Task<WorkflowStep?> GetByWorkflowIdAndStepCode(string workflowId, string stepCode);
    Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model);
    Task<IPagedList<WorkflowStep>> GetByWorkflowId(string workflowId);
    Task<WorkflowStep> GetByWorkflowIdAndStepCodeAndStepOrder(
        string workflowId,
        string stepCode,
        int stepOrder
    );
}
