using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class AuthenticateWorkflowService : BaseQueueService
{
    private readonly IAuthenticateService _authenticateService =
        EngineContext.Current.Resolve<IAuthenticateService>();

    public async Task<WorkflowScheme> AuthenJWT(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<AuthenJWTModel>();

        return await Invoke<AuthenJWTModel>(
            workflow,
            async () =>
            {
                var response = await _authenticateService.AuthenJwt(
                    model,
                    workflow.Request.RequestHeader.UtcSendTime,
                    workflow.Request.RequestHeader.ServerIp
                );
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> DigitalHashPassword(WorkflowScheme workflow)
    {
        return await Invoke<AuthenJWTModel>(
            workflow,
            async () =>
            {
                var response = await _authenticateService.DigitalHashPassword(workflow);
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> GetUserLoginAppPortal(WorkflowScheme workflow)
    {
        return await Invoke<AuthenJWTModel>(
            workflow,
            async () =>
            {
                var response = await _authenticateService.DigitalHashPassword(workflow);
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> Logout(WorkflowScheme workflow)
    {
        return await Invoke<AuthenJWTModel>(
            workflow,
            async () =>
            {
                var response = await _authenticateService.Logout();
                return response;
            }
        );
    }
}
