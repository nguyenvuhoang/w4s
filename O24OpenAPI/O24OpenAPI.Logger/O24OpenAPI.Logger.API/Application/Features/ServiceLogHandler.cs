using LinKit.Core.Cqrs;
using LinKit.Json.Runtime;
using Newtonsoft.Json;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Services.Events;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

public static class ServiceLogExtensions
{
    public static object GetServiceLogcommand(this string logType, string input)
    {
        return logType switch
        {
            "HTTP_LOG" => input.FromJson<CreateHttLogCommand>(),
            "WORKFLOW_LOG" => input.FromJson<CreateWorkflowLogCommand>(),
            "WORKFLOW_STEP_LOG" => input.FromJson<CreateWorkflowStepLogCommand>(),
            _ => throw new Exception($"Logtype {logType} is not supported."),
        };
    }
}

public class CreateHttLogCommand : HttpLog, ICommand { }
public class CreateWorkflowLogCommand : WorkflowLog, ICommand { }
public class CreateWorkflowStepLogCommand : WorkflowStepLog, ICommand { }

public class ServiceLogHandler([FromKeyedServices("log")] IMediator mediator) : IConsumer<LogEvent>
{
    public async Task HandleEvent(LogEvent eventMessage)
    {
        try
        {
            object command = eventMessage.LogType.GetServiceLogcommand(eventMessage.TextData);
            await mediator.SendAsync((ICommand)command);
        }
        catch (Exception ex)
        {
            ServiceLog logE = new()
            {
                LogLevelId = 40,
                ServiceId = "LOG",
                ShortMessage = ex.Message,
                FullMessage = ex.StackTrace,
                Data = JsonConvert.SerializeObject(eventMessage),
            };
            await EngineContext.Current.Resolve<IServiceLogRepository>().InsertAsync(logE);
        }
    }
}
