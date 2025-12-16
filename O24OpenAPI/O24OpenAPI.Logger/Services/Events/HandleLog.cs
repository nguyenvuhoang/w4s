using MediatR;
using Newtonsoft.Json;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Services.CommandHandler;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Logger.Utils;
using O24OpenAPI.Framework.Services.Events;

namespace O24OpenAPI.Logger.Services.Events;

/// <summary>
/// The handle log class
/// </summary>
/// <seealso cref="IConsumer{LogEvent}"/>
public class HandleLog : IConsumer<LogEvent>
{
    /// <summary>
    /// The mediator
    /// </summary>
    private readonly IMediator _mediator = EngineContext.Current.Resolve<IMediator>();

    /// <summary>
    /// Handles the event using the specified event message
    /// </summary>
    /// <param name="eventMessage">The event message</param>
    public async Task HandleEvent(LogEvent eventMessage)
    {
        try
        {
            var type = TypeLogUtils.GetTypeLog(eventMessage.LogType);
            var logCommandType = typeof(LogCommand<>).MakeGenericType(type);
            var logCommand = Activator.CreateInstance(logCommandType, eventMessage.TextData);
            await _mediator.Send((IRequest)logCommand);
        }
        catch (Exception ex)
        {
            var logE = new ServiceLog()
            {
                LogLevelId = 40,
                ServiceId = "LOG",
                ShortMessage = ex.Message,
                FullMessage = ex.StackTrace,
                Data = JsonConvert.SerializeObject(eventMessage),
            };
            await EngineContext.Current.Resolve<ILogService<ServiceLog>>().AddAsync(logE);
        }
    }
}
