using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

public class AddApplicationLog(IApplicationLogRepository applicationLogRepository)
    : IIntegrationEventHandler<LoggingEvent>
{
    public Task Handle(LoggingEvent @event)
    {
        try
        {
            ApplicationLog applicationLog = @event.LogEntryModel.ToApplicationLog();
            return applicationLogRepository.InsertAsync(applicationLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }
    }
}
