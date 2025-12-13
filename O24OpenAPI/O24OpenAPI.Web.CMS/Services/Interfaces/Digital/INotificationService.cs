using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface INotificationService
{
    Task<List<D_NOTIFICATION>> GetListByAppAndType(string appType, string type);
    Task<List<D_NOTIFICATION>> GetListByApp(string appType);
    Task SendBulkPushNotificationsAsync(List<string> expoPushTokens, string title, string body);
    Task SendNotificationAsync(
        string token,
        string title,
        string body,
        Dictionary<string, string> data = null
    );
    Task<IPagedList<D_NOTIFICATION>> Search(NotificationSearchModel model);
    Task Insert(D_NOTIFICATION notification);
    Task<int> GetUnreadCount(string userCode, string appType);
    Task<D_NOTIFICATION> GetById(int id);
    Task Update(D_NOTIFICATION notification);
    Task ProcessNotificationTimeout();
    Task CompleteNotification(int id);
}
