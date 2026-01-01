using O24OpenAPI.Client.Events.EventData;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Events;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

public class HandleLogWorkflowStepEvent : IConsumer<StepExecutionEvent>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="eventMessage"></param>
    /// <returns></returns>
    public async Task HandleEvent(StepExecutionEvent eventMessage)
    {
        await EngineContext
            .Current.Resolve<IWorkflowStepLogRepository>()
            .UpdateShouldNotAwaitStepInfo(eventMessage.WFScheme);
    }
}
