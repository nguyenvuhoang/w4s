using LinKit.Core.Cqrs;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.W4S.API.Application.Features;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.W4S.API.Queues;

public class SampleQueue(IMediator mediator) : BaseQueue
{
    public async Task<WFScheme> ProcessSample(WFScheme scheme)
    {
        return await Invoke<BaseTransactionModel>(scheme, async () =>
        {
            var command = await scheme.ToModel<CreateSampleCommand>();
            return await mediator.SendAsync(command);
        });
    }
}
