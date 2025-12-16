using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class LoadFormWorkflowService(ILoadFormService formService) : BaseQueueService
{
    private readonly ILoadFormService _formService = formService;

    public async Task<WorkflowScheme> LoadFormAndRoleTask(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<FormModelRequest>();
        return await Invoke<FormModelRequest>(
            workflow,
            async () =>
            {
                var response = await _formService.LoadFormAndRoleTask(model);
                return response;
            }
        );
    }
}
