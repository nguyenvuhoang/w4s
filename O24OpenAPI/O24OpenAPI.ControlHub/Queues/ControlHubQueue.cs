using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class ControlHubQueue : BaseQueue
{
    private readonly IUserAgreementService _userAgreementService = EngineContext.Current.Resolve<IUserAgreementService>();
    /// <summary>
    /// Load User Agreement
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> LoadUserAgreement(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<LoadUserAgreementRequestModel>();
        return await Invoke<LoadUserAgreementRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAgreementService.LoadUserAgreementAsync(model);
                return response;
            }
        );
    }
}
