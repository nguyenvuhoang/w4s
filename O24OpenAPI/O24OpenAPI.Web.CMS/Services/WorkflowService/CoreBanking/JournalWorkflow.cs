using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class JournalWorkflow(ICoreJournalService coreJournalService) : BaseQueueService
{
    private readonly ICoreJournalService _coreJournalService = coreJournalService;

    public async Task<WorkflowScheme> View(WorkflowScheme workflowScheme)
    {
        var model = workflowScheme.ToO9Model();
        return await Invoke<BaseTransactionModel>(
            workflowScheme,
            async () => await _coreJournalService.ViewF8(model),
            "ViewF8"
        );
    }
}
