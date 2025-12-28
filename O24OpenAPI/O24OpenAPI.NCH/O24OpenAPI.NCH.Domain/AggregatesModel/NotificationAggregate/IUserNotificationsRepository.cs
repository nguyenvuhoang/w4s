using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface IUserNotificationsRepository : IRepository<UserNotifications>
{
    Task<IReadOnlyList<UserNotifications>> GetByUserAsync(long userId);
}
