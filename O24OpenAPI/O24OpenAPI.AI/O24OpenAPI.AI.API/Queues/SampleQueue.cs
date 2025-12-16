using LinKit.Core.Cqrs;
using O24OpenAPI.AI.API.Application.Features;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.AI.API.Queues;

public class SampleQueue(IMediator mediator) : BaseQueue
{
    public async Task<WFScheme> ProcessSample(WFScheme scheme)
    {
        return await Invoke<BaseTransactionModel>(
            scheme,
            async () =>
            {
                var command = await scheme.ToModel<CreateSampleCommand>();
                return await mediator.SendAsync(command);
            }
        );
    }
}
