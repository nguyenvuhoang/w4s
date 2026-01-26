using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface IUserNotificationsRepository : IRepository<UserNotification>
{
    Task<IReadOnlyList<UserNotification>> GetByUserAsync(long userId);
}
