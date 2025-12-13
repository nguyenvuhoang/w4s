using O24OpenAPI.Web.CMS.Models.Neptune;
using O24OpenAPI.Web.CMS.Services.NeptuneService;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService.CoreBanking;

public class NeptuneCbsWorkflow(INeptuneCBSService neptuneCBSService) : BaseQueueService
{
    private readonly INeptuneCBSService _neptuneCBSService = neptuneCBSService;

    public async Task<WorkflowScheme> ExecuteWorkflow(WorkflowScheme workflowScheme)
    {
        var model = await workflowScheme.ToModel<ExecuteWorkflowNeptuneModel>();
        return await Invoke<BaseTransactionModel>(
            workflowScheme,
            async () =>
            {
                return await _neptuneCBSService.ExecuteWorkflow(model);
            }
        );
    }
}
