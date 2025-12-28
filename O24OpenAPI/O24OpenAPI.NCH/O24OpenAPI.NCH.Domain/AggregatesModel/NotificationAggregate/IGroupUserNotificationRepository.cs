using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface IGroupUserNotificationRepository : IRepository<GroupUserNotification>
{
    Task<IReadOnlyList<GroupUserNotification>> GetByGroupAsync(long groupId);
}
