using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Queues;

public class AccountChartQueue : BaseQueue
{
    private readonly IAccountChartService _accountChartService =
        EngineContext.Current.Resolve<IAccountChartService>();

    public async Task<WFScheme> Create(WFScheme workflow)
    {
        var model = await workflow.ToModel<CreateAccountChartRequestModel>();
        return await Invoke<CreateAccountChartRequestModel>(
            workflow,
            async () =>
            {
                var rs = await _accountChartService.CreateAsync(model);
                return rs;
            }
        );
    }

    public async Task<WFScheme> Delete(WFScheme workflow)
    {
        var model = await workflow.ToModel<AccountChartDefaultModel>();
        return await Invoke<AccountChartDefaultModel>(
            workflow,
            async () =>
            {
                var rs = await _accountChartService.DeleteAsync(model);
                return rs;
            }
        );
    }
}
