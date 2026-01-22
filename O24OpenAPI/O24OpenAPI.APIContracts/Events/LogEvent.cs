using O24OpenAPI.Contracts.Events;
using O24OpenAPI.Contracts.Models;

namespace O24OpenAPI.APIContracts.Events;

public class LoggingEvent : IntegrationEvent
{
    public LogEntryModel? LogEntryModel { get; set; }
}
