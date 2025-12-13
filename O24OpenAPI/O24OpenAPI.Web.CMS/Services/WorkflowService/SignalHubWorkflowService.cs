using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Services;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class SignalHubWorkflowService : BaseQueueService
{
    private readonly IHubContext<SignalHubService> _signal = EngineContext.Current.Resolve<
        IHubContext<SignalHubService>
    >();

    public async Task<WorkflowScheme> SignalSendToUser(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var model = await workflow.ToModel<SignalSendToUserModel>();
                await SignalHubService.SignalSendToUser(
                    _signal,
                    model.Channel,
                    model.Message,
                    model.UserCode,
                    model.DeviceId
                );
                return new StatusCompleteResponse();
            }
        );
    }
}
