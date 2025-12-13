using O24OpenAPI.Contracts.Events;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.EventBus.Abstractions;
using O24OpenAPI.Web.Framework.DBContext;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.DataWarehouse.Services.Events;

public class CDCEventHandler(ServiceDBContext serviceDBContext)
    : IIntegrationEventHandler<CDCEvent>
{
    private readonly ServiceDBContext _serviceDBContext = serviceDBContext;

    public async Task Handle(CDCEvent @event)
    {
        try
        {
            var source = StringUtils.Coalesce(@event.source, "CDC");
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
