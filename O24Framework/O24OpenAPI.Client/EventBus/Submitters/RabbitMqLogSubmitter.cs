using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Logging.Abstractions;
using O24OpenAPI.Logging.Extensions;
using Serilog.Events;

namespace O24OpenAPI.Client.EventBus.Submitters;

public class RabbitMqLogSubmitter(IServiceProvider serviceProvider) : ILogSubmitter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task SubmitAsync(IEnumerable<LogEvent> logEvents)
    {
        if (!logEvents.Any())
        {
            return;
        }

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            foreach (var logEvent in logEvents)
            {
                var dto = logEvent.ToLogEntryModel();
                var @event = new LoggingEvent() { LogEntryModel = dto };
                var cancellationToken = new CancellationToken();
                await eventBus.PublishAsync(@event, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[FATAL][RabbitMqLogSubmitter] Failed to submit logs via RabbitMQ. Error: {ex.Message}"
            );
            await Task.Delay(5000);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
