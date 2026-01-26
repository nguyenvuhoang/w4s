using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Contracts.Events;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Framework.DBContext;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.DWH.API.Application.IntergrationEvents.EventHandling;

public class CDCEventHandler(ServiceDBContext serviceDBContext) : IIntegrationEventHandler<CDCEvent>
{
    private readonly ServiceDBContext _serviceDBContext = serviceDBContext;

    public async Task Handle(CDCEvent @event)
    {
        try
        {
            string source = StringUtils.Coalesce(@event.source, "CDC");
            BusinessLogHelper.Info($"===========Start CDC Event {source}===============");

            var sqlAuditLog = new SQLAuditLog
            {
                CommandName = $"CDC_{@event.table_name}",
                CommandType = @event.action,
                SourceService = source,
                ExecutedBy = source,
            };
            await _serviceDBContext.ExecuteDML(@event.sql, sqlAuditLog);
        }
        catch (Exception ex)
        {
            await ex.LogError(@event);
        }
    }
}
