using System.Collections.Concurrent;
using System.Threading.Channels;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.O24OpenAPIClient.Events.EventData;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Services;
using O24OpenAPI.Web.Framework.Services.Events;

namespace O24OpenAPI.DataWarehouse.Services.Events;

public class EntityEventHandler : IConsumer<EntityEvent>
{
    private static readonly ConcurrentDictionary<string, Channel<EntityEvent>> _queues = new();

    public EntityEventHandler()
    {
        foreach (var (tableName, queue) in _queues)
        {
            _ = ProcessQueue(tableName, queue);
        }
    }

    public async Task HandleEvent(EntityEvent eventMessage)
    {
        var queue = _queues.GetOrAdd(
            eventMessage.table_name,
            _ => CreateQueue(eventMessage.table_name)
        );
        await queue.Writer.WriteAsync(eventMessage);
    }

    private static Channel<EntityEvent> CreateQueue(string tableName)
    {
        var channel = Channel.CreateUnbounded<EntityEvent>();
        _ = ProcessQueue(tableName, channel);
        return channel;
    }

    private static async Task ProcessQueue(string tableName, Channel<EntityEvent> queue)
    {
        await foreach (var eventMessage in queue.Reader.ReadAllAsync())
        {
            try
            {
                await TaskUtils.RunInNewScope(async () =>
                {
                    var actionRequestModel = new ActionRequestModel(eventMessage);
                    var service = EngineContext.Current.Resolve<IExecuteQueryService>();
                    await service.ExecuteDML(actionRequestModel);
                });
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(eventMessage);
            }
        }
    }
}
