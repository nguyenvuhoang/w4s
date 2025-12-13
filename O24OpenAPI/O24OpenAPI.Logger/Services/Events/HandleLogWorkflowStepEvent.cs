using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Events.EventData;
using O24OpenAPI.Web.Framework.Services.Events;

namespace O24OpenAPI.Logger.Services.Events;

/// <summary>
///
/// </summary>
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
            .Current.Resolve<IWorkflowStepLogService>()
            .UpdateShouldNotAwaitStepInfo(eventMessage.WFScheme);
    }
}
