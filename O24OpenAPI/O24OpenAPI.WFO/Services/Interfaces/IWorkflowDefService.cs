using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Services.Interfaces;

public interface IWorkflowDefService
{
    Task<WorkflowDef> GetByIdAsync(int id);
    Task<WorkflowDef> GetByWorkflowIdAsync(string workflowId, string channelId);
    Task<IList<WorkflowDef>> GetAllAsync();
    Task<WorkflowDef> AddAsync(WorkflowDef workflowDef);
    Task UpdateAsync(WorkflowDef workflowDef);
    Task DeleteAsync(WorkflowDef workflowDef);
    Task<IPagedList<WorkflowDef>> SimpleSearch(SimpleSearchModel model);
}
