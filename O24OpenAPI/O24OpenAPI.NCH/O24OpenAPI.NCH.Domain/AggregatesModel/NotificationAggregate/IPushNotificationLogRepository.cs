using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface IPushNotificationLogRepository : IRepository<PushNotificationLog>
{
    Task<IReadOnlyList<PushNotificationLog>> GetByUserAsync(long userId);
}
