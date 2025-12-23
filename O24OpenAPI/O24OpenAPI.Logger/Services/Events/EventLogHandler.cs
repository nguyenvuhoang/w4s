using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Mapper;

namespace O24OpenAPI.Logger.Services.Events;

public class EventLogHandler : IIntegrationEventHandler<LoggingEvent>
{
    private readonly IRepository<ApplicationLog> _applicationLogRepository =
        EngineContext.Current.Resolve<IRepository<ApplicationLog>>();

    public Task Handle(LoggingEvent @event)
    {
        try
        {
            var applicationLog = @event.LogEntryModel.MapToApplicationLog();
            return _applicationLogRepository.InsertAsync(applicationLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }
    }
}
