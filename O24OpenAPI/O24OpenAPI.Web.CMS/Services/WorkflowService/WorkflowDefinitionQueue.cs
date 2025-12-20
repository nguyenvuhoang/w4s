using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class WorkflowDefinitionQueue(IWorkflowDefinitionService workflowDefinitionService)
    : BaseQueueService
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService =
        workflowDefinitionService;

    public async Task<WorkflowScheme> SimpleSearch(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();
        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var workflowDefinitions = await _workflowDefinitionService.SimpleSearch(model);
                return workflowDefinitions.ToPagedListModel<
                    WorkflowDefinition,
                    WorkflowDefinitionSearchResponseModel
                >();
            }
        );
    }

    public async Task<WorkflowScheme> AdvancedSearch(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<WorkflowDefinitionSearchModel>();
        return await Invoke<WorkflowDefinitionSearchModel>(
            workflow,
            async () =>
            {
                var workflowDefinitions = await _workflowDefinitionService.AdvancedSearch(model);
                return workflowDefinitions.ToPagedListModel<
                    WorkflowDefinition,
                    WorkflowDefinitionSearchResponseModel
                >();
            }
        );
    }
}
