using LinKit.Core.Cqrs;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.W4S.API.Application.Features;

namespace O24OpenAPI.W4S.API.Queues;

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
