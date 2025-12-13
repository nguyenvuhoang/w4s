using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using System.Text;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class NotificationService : INotificationService
{
    private readonly IRepository<D_NOTIFICATION> _contextRepository;
    private const string ExpoPushApiUrl = "https://exp.host/--/api/v2/push/send";
    private readonly HttpClient _httpClient;
    private readonly FirebaseMessaging _messaging;
    private readonly CMSSetting _cmsSetting;

    public NotificationService(
        IRepository<D_NOTIFICATION> contextRepository,
        IWebHostEnvironment environment,
        CMSSetting cmsSetting
    )
    {
        _contextRepository = contextRepository;
        _httpClient = new HttpClient();
        var firebaseConfigPath = cmsSetting.FirebaseConfigPath;
        var fullPath = Path.Combine(environment.ContentRootPath, firebaseConfigPath);
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(
                new AppOptions() { Credential = GoogleCredential.FromFile(fullPath) }
            );
        }

        _messaging = FirebaseMessaging.DefaultInstance;
        _cmsSetting = cmsSetting;
    }

    public async Task<D_NOTIFICATION> GetById(int id)
    {
        return await _contextRepository.GetById(id);
    }

    public async Task<List<D_NOTIFICATION>> GetListByApp(string appType)
    {
        return await _contextRepository.Table.Where(x => x.AppType == appType).ToListAsync();
    }

    public async Task<List<D_NOTIFICATION>> GetListByAppAndType(string appType, string type)
    {
        return await _contextRepository
            .Table.Where(x => x.AppType == appType && x.NotificationType == type)
            .ToListAsync();
    }

    public async Task<IPagedList<D_NOTIFICATION>> Search(NotificationSearchModel model)
    {
        var query = _contextRepository.Table.AsQueryable();
        string channelId = StringUtils.Coalesce(model.AppType, model.ChannelId);
        if (!string.IsNullOrEmpty(channelId))
        {
            query = query.Where(x => x.AppType == channelId);
        }
        if (!string.IsNullOrEmpty(model.NotificationType))
        {
            query = query.Where(x => x.NotificationType == model.NotificationType);
        }
        string userCode = StringUtils.Coalesce(model.UserCode, SessionUtils.GetUserCode());
        if (!string.IsNullOrEmpty(userCode))
        {
            query = query.Where(x => x.UserCode == userCode);
        }
        query = query.OrderByDescending(x => x.DateTime);
        var data = await query.ToPagedList(model.PageIndex, model.PageSize);
        return data;
    }

    public async Task Insert(D_NOTIFICATION notification)
    {
        await _contextRepository.Insert(notification);
    }

    public async Task Update(D_NOTIFICATION notification)
    {
        await _contextRepository.Update(notification);
    }

    public async Task<int> GetUnreadCount(string userCode, string appType)
    {
        return await _contextRepository
            .Table.Where(x => x.UserCode == userCode && x.AppType == appType && x.IsRead == false)
            .CountAsync();
    }

    public async Task CompleteNotification(int id)
    {
        var notification = await _contextRepository.GetById(id);
        notification.IsProcessed = true;
        await Update(notification);
    }

    public async Task SendBulkPushNotificationsAsync(
        List<string> expoPushTokens,
        string title,
        string body
    )
    {
        var messages = expoPushTokens
            .Select(token => new
            {
                to = token,
                title = title,
                body = body,
                sound = "default",
                badge = 1,
            })
            .ToList();

        var payload = JsonConvert.SerializeObject(messages);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(ExpoPushApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Failed to send push notifications. Status: {response.StatusCode}, Response: {responseContent}"
            );
        }
    }

    public async Task SendNotificationAsync(
        string token,
        string title,
        string body,
        Dictionary<string, string> data = null
    )
    {
        data ??= new Dictionary<string, string>();
        data["title"] = title;
        data["body"] = body;
        var message = new Message() { Token = token, Data = data };

        try
        {
            var response = await _messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    public async Task SendNotificationToTopicAsync(
        string topic,
        string title,
        string body,
        Dictionary<string, string> data = null
    )
    {
        var message = new Message()
        {
            Topic = topic,
            Notification = new Notification() { Title = title, Body = body },
            Data = data,
        };

        try
        {
            var response = await _messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message to topic: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to topic: {ex.Message}");
        }
    }

    public async Task ProcessNotificationTimeout()
    {
        var list = await _contextRepository
            .Table.Where(x =>
                (DateTime.Now.ToLocalTime() - x.DateTime).TotalSeconds
                    > _cmsSetting.NotificationExpiredInSeconds
                && x.IsProcessed == false
            )
            .ToListAsync();

        foreach (var item in list)
        {
            item.IsProcessed = true;
            await Update(item);
        }
    }
}
