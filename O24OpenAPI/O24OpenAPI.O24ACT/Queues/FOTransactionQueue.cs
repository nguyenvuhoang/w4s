using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Queues;

public class FOTransactionQueue : BaseQueue
{
    private readonly IFOTransactionServices _foTransactionService =
        EngineContext.Current.Resolve<IFOTransactionServices>();

    public async Task<WFScheme> ExcuteAccountingRule(WFScheme workflow)
    {
        var model = await workflow.ToModel<ExcuteAccountingRuleModel>();
        return await Invoke<ExcuteAccountingRuleModel>(
            workflow,
            async () =>
            {
                var rs = await _foTransactionService.ExcuteAccountingRuleAsync(model);
                return rs;
            }
        );
    }
}
