using O24OpenAPI.Core;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Response;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface INotificationService
{
    Task<List<Notification>> GetListByApp(string appType);
    Task SendBulkPushNotificationsAsync(List<string> expoPushTokens, string title, string body);
    Task SendNotificationAsync(
        string token,
        string title,
        string body,
        Dictionary<string, string> data = null
    );
    Task<IPagedList<Notification>> Search(NotificationSearchModel model);
    Task Insert(Notification notification);
    Task<int> GetUnreadCount(string userCode, string appType);
    Task<Notification> GetById(int id);
    Task Update(Notification notification);
    Task ProcessNotificationTimeout();
    Task CompleteNotification(int id);
    Task<List<Notification>> GetListByAppAndType(string appType, string type);
    /// <summary>
    /// LogInformation
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="appType"></param>
    /// <param name="notificationType"></param>
    /// <param name="templateID"></param>
    /// <param name="redirect"></param>
    /// <param name="dataSending"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns></returns>
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
        string imageUrl = "");
    /// <summary>
    /// SendNotification
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SendNotification(NotificationRequestModel model);
    /// <summary>
    /// SearchAsync
    /// </summary>
    /// <param name="model"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<NotificationSearchResponseModel> SearchAsync(NotificationSearchModel model, CancellationToken ct = default);
    /// <summary>
    /// Asynchronously sends mobile device information to the server.
    /// </summary>
    /// <param name="model">The request model containing the mobile device details to be sent. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the device
    /// information was sent successfully; otherwise, <see langword="false"/>.</returns>
    Task<bool> SendMobileDeviceAsync(SendMobileDeviceRequestModel model);

}
