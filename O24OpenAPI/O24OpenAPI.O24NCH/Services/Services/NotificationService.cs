using FirebaseAdmin;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.O24NCH.Config;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Request.Mail;
using O24OpenAPI.O24NCH.Models.Request.Telegram;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24NCH.Utils;
using System.Text;
using System.Text.RegularExpressions;

namespace O24OpenAPI.O24NCH.Services.Services;

public partial class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _contextRepository;
    private const string ExpoPushApiUrl = "https://exp.host/--/api/v2/push/send";
    private readonly HttpClient _httpClient;
    private readonly FirebaseAdmin.Messaging.FirebaseMessaging _messaging;
    private readonly O24NCHSetting _nchApiSetting;
    private readonly IRepository<PushNotificationLog> _pushNotificationLog;
    private readonly ISMSService _smsService;
    private readonly IEmailService _mailService;
    private readonly ITelegramService _telegramService;
    private readonly INotificationTemplateService _notificationTemplateService;

    public NotificationService(
        IRepository<Notification> contextRepository,
        IWebHostEnvironment environment,
        O24NCHSetting nchApiSetting,
        IRepository<PushNotificationLog> pushNotificationLog,
        ISMSService smsService,
        IEmailService mailService,
        ITelegramService telegramService,
        INotificationTemplateService notificationTemplateService
    )
    {
        _httpClient = new HttpClient();
        _nchApiSetting = nchApiSetting;
        _contextRepository = contextRepository;
        _notificationTemplateService = notificationTemplateService;
        var firebaseConfigPath = _nchApiSetting.FirebaseConfigPath;
        var fullPath = Path.Combine(environment.ContentRootPath, firebaseConfigPath);

        Console.WriteLine(
            $"[Firebase Init] ConfigPath='{firebaseConfigPath}', FullPath='{fullPath}', Exists={File.Exists(fullPath)}"
        );

        string projectIdFromJson = null;
        try
        {
            using var fs = File.OpenRead(fullPath);
            using var sr = new StreamReader(fs);
            var json = sr.ReadToEnd();
            var jo = Newtonsoft.Json.Linq.JObject.Parse(json);
            projectIdFromJson = (string)jo["project_id"];
        }
        catch (Exception exParse)
        {
            Console.WriteLine(
                $"[Firebase Init] WARN: Cannot parse project_id from JSON. {exParse.GetType().Name}: {exParse.Message}"
            );
        }

        var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(fullPath);
        string projectId = null;
        try
        {
            if (
                credential.UnderlyingCredential
                is Google.Apis.Auth.OAuth2.ServiceAccountCredential sac
            )
            {
                projectId = sac.ProjectId;
            }
        }
        catch (Exception exCredPid)
        {
            Console.WriteLine(
                $"[Firebase Init] WARN: Cannot read ProjectId from credential. {exCredPid.GetType().Name}: {exCredPid.Message}"
            );
        }
        projectId ??= projectIdFromJson;
        projectId ??= _nchApiSetting.FirebaseProjectId;
        if (FirebaseApp.DefaultInstance == null)
        {
            var app = FirebaseApp.Create(
                new AppOptions { Credential = credential, ProjectId = projectId }
            );
            Console.WriteLine(
                $"[Firebase Init] App created. Name={app.Name}, ProjectId='{app.Options.ProjectId ?? "(null)"}'"
            );
        }
        else
        {
            var app = FirebaseApp.DefaultInstance;
            Console.WriteLine(
                $"[Firebase Init] Already exists. Name={app.Name}, ProjectId={app.Options.ProjectId}"
            );
        }

        _messaging = FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance;
        _pushNotificationLog = pushNotificationLog;
        _smsService = smsService;
        _mailService = mailService;
        _telegramService = telegramService;
    }

    public async Task<Notification> GetById(int id)
    {
        return await _contextRepository.GetById(id);
    }

    public async Task<List<Notification>> GetListByApp(string appType)
    {
        return await _contextRepository.Table.Where(x => x.AppType == appType).ToListAsync();
    }

    public async Task<List<Notification>> GetListByAppAndType(string appType, string type)
    {
        return await _contextRepository
            .Table.Where(x => x.AppType == appType && x.NotificationType == type)
            .ToListAsync();
    }

    public async Task<IPagedList<Notification>> Search(NotificationSearchModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserCode))
        {
            return new PagedList<Notification>([], model.PageIndex, model.PageSize);
        }

        var pageSize = Math.Clamp(model.PageSize, 1, 100);
        var targetChannel = StringUtils.Coalesce(model.AppType, model.ChannelId);

        IQueryable<Notification> query = _contextRepository.Table;

        query = query.Where(x => x.UserCode == model.UserCode);

        if (!string.IsNullOrWhiteSpace(targetChannel))
        {
            query = query.Where(x => x.AppType == targetChannel);
        }

        if (!string.IsNullOrWhiteSpace(model.NotificationType))
        {
            query = query.Where(x => x.NotificationType == model.NotificationType);
        }

        query = query.OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Id);

        var data = await query.ToPagedList(model.PageIndex, pageSize);
        return data;
    }

    public async Task Insert(Notification notification)
    {
        await _contextRepository.InsertAsync(notification);
    }

    public async Task Update(Notification notification)
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
                title,
                body,
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

    /// <summary>
    /// Send a push notification to a specific device using Firebase Cloud Messaging (FCM).
    /// </summary>
    /// <param name="token"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task SendNotificationAsync(
        string token,
        string title,
        string body,
        Dictionary<string, string> data = null
    )
    {
        data ??= [];
        data["title"] = title;
        data["body"] = body;

        var log = new PushNotificationLog
        {
            Token = token,
            Title = title,
            Body = body,
            Data = JsonConvert.SerializeObject(data),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
        };

        try
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Token = token,
                Data = data,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body,
                },
                Android = new FirebaseAdmin.Messaging.AndroidConfig
                {
                    Priority = FirebaseAdmin.Messaging.Priority.High,
                    Notification = new FirebaseAdmin.Messaging.AndroidNotification
                    {
                        Sound = "emi.wav",
                        ChannelId = "emi_default_channel",
                    },
                },
                Apns = new FirebaseAdmin.Messaging.ApnsConfig
                {
                    Aps = new FirebaseAdmin.Messaging.Aps
                    {
                        ContentAvailable = true,
                        Sound = "emi.wav",
                    },
                },
            };

            log.RequestMessage = JsonConvert.SerializeObject(message);
            var response = await _messaging.SendAsync(message);
            log.ResponseId = response;
            log.Status = "Sent";
            log.SentAt = DateTime.UtcNow;
        }
        catch (FirebaseAdmin.Messaging.FirebaseMessagingException ex)
        {
            await ex.LogErrorAsync();
            log.Status = "Failed";
            log.ErrorMessage = $"[{ex.MessagingErrorCode}] {ex.Message} | ErrorCode={ex.ErrorCode}";
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            log.Status = "Failed";
            log.ErrorMessage = ex.Message;
        }

        await _pushNotificationLog.InsertAsync(log);
    }

    public async Task SendNotificationToTopicAsync(
        string topic,
        string title,
        string body,
        Dictionary<string, string> data = null
    )
    {
        var message = new FirebaseAdmin.Messaging.Message()
        {
            Topic = topic,
            Notification = new FirebaseAdmin.Messaging.Notification()
            {
                Title = title,
                Body = body,
            },
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
                    > _nchApiSetting.NotificationExpiredInSeconds
                && x.IsProcessed == false
            )
            .ToListAsync();

        foreach (var item in list)
        {
            item.IsProcessed = true;
            await Update(item);
        }
    }

    /// <summary>
    /// Log notification information into the database.
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="appType"></param>
    /// <param name="notificationType"></param>
    /// <param name="templateID"></param>
    /// <param name="redirect"></param>
    /// <param name="dataSending"></param>
    /// <param name="notificationCategory"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task<int> LogInformation(
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
    )
    {
        try
        {
            var entity = new Notification
            {
                UserCode = userCode,
                AppType = appType,
                NotificationType = notificationType,
                TemplateID = templateID,
                Redirect = redirect,
                DataValue = JsonConvert.SerializeObject(dataSending),
                IsRead = false,
                IsPushed = false,
                DateTime = DateTime.Now,
                IsProcessed = false,
                NotificationCategory = notificationCategory,
                Message = message,
                Title = title,
                ImageUrl = imageUrl,
            };

            await Insert(entity);

            return entity.Id;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return -1;
        }
    }

    public async Task<bool> SendNotification(NotificationRequestModel model)
    {
        var workContext = EngineContext.Current.Resolve<WorkContext>();
        model.RefId = workContext.ExecutionId ?? Guid.NewGuid().ToString();
        try
        {
            var type = (model?.NotificationType ?? "").Trim().ToUpperInvariant();

            switch (type)
            {
                case "MAIL":
                    return await _mailService.SendEmailAsync(MapToSendMailModel(model));

                case "SMS":
                    var sms = model as SMSRequestModel ?? MapToSms(model);
                    return await _smsService.SendSMS(sms);

                case "TELE":
                    var tele = model as TelegramSendModel ?? MapToTelegram(model);
                    return await _telegramService.SendMessage(tele);

                default:
                    throw new NotSupportedException(
                        $"Notification type '{model?.NotificationType}' is not supported."
                    );
            }
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync(
                new Dictionary<string, object?>
                {
                    ["RuntimeType"] = model?.GetType().FullName,
                    ["NotificationType"] = model?.NotificationType,
                }
            );
            return false;
        }
    }

    private static SMSRequestModel MapToSms(NotificationRequestModel m)
    {
        return m is null
            ? throw new ArgumentNullException(nameof(m))
            : new SMSRequestModel
            {
                NotificationType = "SMS",
                Purpose = m.Purpose,
                PhoneNumber = m.PhoneNumber,
                SenderData = m.SenderData ?? [],
                RefId = m.RefId,
                Message = m.Message,
            };
    }

    private static TelegramSendModel MapToTelegram(NotificationRequestModel m)
    {
        return new TelegramSendModel
        {
            NotificationType = "TELE", /*...*/
        };
    }

    private static SendMailRequestModel MapToSendMailModel(NotificationRequestModel model)
    {
        return new SendMailRequestModel
        {
            TemplateId = $"{model.ChannelId}_{model.TemplateId}",
            ConfigId = "main_mail",
            Receiver = model.Email,
            DataTemplate = model.DataTemplate ?? [],
            AttachmentBase64Strings = model.AttachmentBase64Strings ?? [],
            AttachmentFilenames = model.AttachmentFilenames ?? [],
            MimeEntities = model.MimeEntities ?? [],
            IncludeLogo = true,
            FileIds = model.FileIds ?? [],
        };
    }

    public async Task<NotificationSearchResponseModel> SearchAsync(
        NotificationSearchModel model,
        CancellationToken ct = default
    )
    {
        var paged = await Search(model);
        if (paged == null || paged.Count == 0)
        {
            return new NotificationSearchResponseModel([], paged);
        }

        var templateIds = paged.Select(x => x.TemplateID).Distinct().ToList();
        var templates = await _notificationTemplateService.GetByTemplateIdsAsync(templateIds);
        var templateMap = templates.ToDictionary(t => t.TemplateID);

        var language = string.IsNullOrWhiteSpace(model.Language)
            ? "en"
            : model.Language.Trim().ToLowerInvariant();
        var list = new List<NotificationSearchResponse>(paged.Count);

        foreach (var item in paged)
        {
            var obj = new NotificationSearchResponse(item);

            if (
                templateMap.TryGetValue(item.TemplateID, out var template)
                && template?.Body != null
            )
            {
                obj.IsShowButton = template.IsShowButton;
                obj.Message =
                    template.Body.GetMessage(obj.Data, language)
                    ?? template.Body.GetMessage(obj.Data, "en")
                    ?? string.Empty;
            }
            else
            {
                obj.IsShowButton = false;
                obj.Message = string.Empty;
            }

            obj.TemplateID = item.TemplateID;
            list.Add(obj);
            item.IsRead = true;
            await Update(item);
        }
        var response = new NotificationSearchResponseModel(list, paged);
        return response;
    }

    /// <summary>
    /// Sends a push notification to a mobile device using the information provided in the specified request model.
    /// </summary>
    /// <param name="model">The request model containing the push notification details, including the device push identifier, notification
    /// title, message, and language. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the notification
    /// was sent successfully; otherwise, an exception is thrown.</returns>
    public async Task<bool> SendMobileDeviceAsync(SendMobileDeviceRequestModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var pushId = model.PushId?.Trim();

        if (string.IsNullOrEmpty(pushId))
        {
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.FCMTokenIsNotExist,
                model.Language,
                [pushId]
            );
        }

        var title = string.IsNullOrWhiteSpace(model.Title) ? "Notification" : model.Title.Trim();

        var body = string.IsNullOrWhiteSpace(model.Message)
            ? "TEST"
            : NormalizeNotificationMessage(model.Message);
        body = Utils.StringExtensions.AutoFormatBullets(body);
        var imageUrl = model.ImageUrl?.Trim() ?? string.Empty;
        try
        {
            await SendNotificationAsync(pushId, title, body);

            var senderLogId = await LogInformation(
                model.UserCode,
                model.AppCode ?? "MB",
                Code.NotificationTypeCode.FIREBASE,
                templateID: model.TemplateId ?? "UNKNOWN",
                redirect: Code.Common.YES,
                dataSending: string.Empty,
                notificationCategory: Code.Common.GENERAL,
                message: body,
                title: title,
                imageUrl: imageUrl
            );

            if (senderLogId <= 0)
            {
                Console.WriteLine("⚠️ Sender log failed: " + model.UserCode);
                return false;
            }
            return true;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(
                $"Failed to send mobile notification. PushId={pushId}, Title={title}"
            );
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Error.SendEmailFailed,
                model.Language,
                [pushId, ex.Message]
            );
        }
    }

    /// <summary>
    /// Normal the notification message by trimming whitespace, replacing multiple spaces and line breaks,
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    private static string NormalizeNotificationMessage(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;

        var text = raw.Trim();

        text = DoubleSpace().Replace(text, " ");

        text = text.Replace("\r\n", "\n").Replace("\r", "\n");

        text = Break().Replace(text, "\n\n");

        if (!string.IsNullOrEmpty(text))
            text = char.ToUpper(text[0]) + text.Substring(1);

        return text;
    }

    [GeneratedRegex(@"(\n){3,}")]
    private static partial Regex Break();

    [GeneratedRegex(@"\s{2,}")]
    private static partial Regex DoubleSpace();
}
