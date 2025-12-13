using Newtonsoft.Json;
using O24OpenAPI.Core.Enums;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient;
using O24OpenAPI.O24OpenAPIClient.Events;
using O24OpenAPI.O24OpenAPIClient.Events.EventData;
using O24OpenAPI.O24OpenAPIClient.Lib;
using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.Web.Framework.Services.Events.Handlers;

/// <summary>
/// The entity event handler class
/// </summary>
/// <seealso cref="IConsumer{EntityActionEvent}"/>
public class EntityEventHandler : IConsumer<EntityActionEvent>
{
    /// <summary>
    /// Handles the event using the specified event message
    /// </summary>
    /// <param name="eventMessage">The event message</param>
    public async Task HandleEvent(EntityActionEvent eventMessage)
    {
        var entityAudit = new EntityAudit
        {
            EntityName = eventMessage.EntityName,
            ActionType = eventMessage.Action.ToString(),
            Data = JsonConvert.SerializeObject(eventMessage.Entity),
            Status = (int)SendEnum.Pending,
        };
        try
        {
            var eventData = new EntityEvent
            {
                table_name = eventMessage.EntityName,
                primary_key = null,
                action_type = eventMessage.Action.ToString(),
                data = eventMessage.Entity,
            };
            var o24Event = new O24OpenAPIEvent<EntityEvent>(
                O24OpenAPIWorkflowEventTypeEnum.EntityAction
            );
            o24Event.EventData.data = eventData;

            var queueClient = Singleton<QueueClient>.Instance;
            await queueClient.SendMessage(
                QueueUtils.GetEventQueueName("DWH"),
                o24Event,
                "event"
            );

            entityAudit.Status = (int)SendEnum.Sent;
        }
        catch
        {
            entityAudit.Status = (int)SendEnum.Failed;
        }
        finally
        {
            await EngineContext.Current.Resolve<IEntityAuditService>().AddAsync(entityAudit);
        }
    }
}
