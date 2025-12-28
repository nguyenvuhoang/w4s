using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IReadOnlyList<Notification>> GetPendingAsync(int take);
    Task<int> LogInformation(
        string userCode,
        string appType,
        string notificationType,
        string templateID,
        string redirect,
        string dataSending,
        string notificationCategory = "BALANCE",
        string message = "",
        string title = "",
        string imageUrl = ""
    );
}
