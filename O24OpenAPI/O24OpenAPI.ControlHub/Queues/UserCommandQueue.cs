using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class UserCommandQueue : BaseQueue
{
    private readonly IUserCommandService _userCommandService =
        EngineContext.Current.Resolve<IUserCommandService>();

    /// <summary>
    /// Get Visible Transactions
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> GetVisibleTransactions(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserCommandRequestModel>();
        return await Invoke<BaseTransactionModel>(
            wFScheme,
            async () =>
            {
                var response = await _userCommandService.GetVisibleTransactions(model.ChannelId);
                return response;
            }
        );
    }
}
