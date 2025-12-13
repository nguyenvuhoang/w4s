using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface IUserNotificationsService
{
    Task<bool> CreateUserNotifications(string userCode, string category);
    /// <summary>
    /// Scan user notifications and send push notifications if there are any new notifications.
    /// </summary>
    /// <returns></returns>
    Task<bool> ScanUserNotification(CancellationToken cancellationToken);
    /// <summary>
    /// Get push notification settings by phone number.
    /// </summary>
    /// <param name="phonenumber"></param>
    /// <returns></returns>
    Task<UserNotifications> GetPushAsync(string phonenumber);
}
