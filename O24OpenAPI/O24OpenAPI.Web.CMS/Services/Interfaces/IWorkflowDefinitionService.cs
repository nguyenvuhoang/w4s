using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IWorkflowDefinitionService
{
    Task<WorkflowDefinition> GetByWFId(string wfId);
    Task<IPagedList<WorkflowDefinition>> SimpleSearch(SimpleSearchModel model);
    Task<IPagedList<WorkflowDefinition>> AdvancedSearch(WorkflowDefinitionSearchModel model);
    Task<List<string>> SeedDataWorkflowDef();
}
