using FirebaseAdmin.Messaging;
using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.NCH.Constant;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Sms;

public class GenerateAndSendOtpCommand : BaseTransactionModel, ICommand<bool>
{
    public Dictionary<string, object> ReceiverData { get; set; } = [];
    public Dictionary<string, object> SenderData { get; set; }
    public string AppType { get; set; }
    public string NotificationType { get; set; }
    public string UserCode { get; set; }
    public string ReceiverCode { get; set; } = string.Empty;
    public string Title { get; set; }
    public string Body { get; set; }
    public string TemplateID { get; set; }
    public string Redirect { get; set; }
    public string SenderPushId { get; set; }
    public string NotificationCategory { get; set; } = "GENERAL";
}

[CqrsHandler]
public class GenerateAndSendOtpHandle(
    INotificationTemplateRepository notificationTemplateRepository,
    ICTHGrpcClientService cTHGrpcClientService,
    INotificationRepository notificationRepository,
    FirebaseMessaging firebaseMessaging,
    IPushNotificationLogRepository pushNotificationLogRepository
) : ICommandHandler<GenerateAndSendOtpCommand, bool>
{
    public async Task<bool> HandleAsync(
        GenerateAndSendOtpCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            int receiverLogId = -1;

            var template = await notificationTemplateRepository.GetByTemplateIdAsync(
                request.TemplateID
            );
            if (template == null)
            {
                Console.WriteLine("⚠️ Template not found: " + request.TemplateID);
                return false;
            }

            var titleDict =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Title ?? "{}")
                ?? [];
            var bodyDict =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Body ?? "{}")
                ?? [];

            string lang = string.IsNullOrWhiteSpace(request.Language) ? "en" : request.Language;
            string rawTitle = titleDict.TryGetValue(lang, out string valuetTitle)
                ? valuetTitle
                : titleDict.GetValueOrDefault("en") ?? "";
            string rawBody = bodyDict.TryGetValue(lang, out string valueBody)
                ? valueBody
                : bodyDict.GetValueOrDefault("en") ?? "";

            var senderDict =
                request.SenderData?.ToDictionary(
                    k => k.Key,
                    dv => dv.Value?.ToString() ?? string.Empty
                )
                ?? [];

            var senderTitle = ReplaceTokens(rawTitle, senderDict);
            var senderBody = ReplaceTokens(rawBody, senderDict);

            var senderToken =
                request.SenderPushId
                ?? await cTHGrpcClientService.GetUserPushIdAsync(request.UserCode);
            if (string.IsNullOrWhiteSpace(senderToken))
            {
                Console.WriteLine("⚠️ Sender token not found for user: " + request.UserCode);
                return false;
            }

            await SendNotificationAsync(senderToken, senderTitle, senderBody);

            var senderLogId = await notificationRepository.LogInformation(
                request.UserCode,
                request.AppCode ?? "MB",
                Code.NotificationTypeCode.FIREBASE,
                template.TemplateID,
                request.Redirect,
                JsonConvert.SerializeObject(senderDict),
                notificationCategory: request.NotificationCategory
            );

            if (senderLogId <= 0)
            {
                Console.WriteLine("⚠️ Sender log failed: " + request.UserCode);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(request.ReceiverCode))
            {
                var receiverToken = await cTHGrpcClientService.GetUserPushIdAsync(
                    request.ReceiverCode
                );
                if (!string.IsNullOrWhiteSpace(receiverToken))
                {
                    var receiverDict =
                        request.ReceiverData != null
                            ? request.ReceiverData.ToDictionary(
                                kv => kv.Key,
                                kv => kv.Value?.ToString() ?? string.Empty
                            )
                            : [];

                    var receiverTitle = ReplaceTokens(rawTitle, receiverDict);
                    var receiverBody = ReplaceTokens(rawBody, receiverDict);

                    await SendNotificationAsync(receiverToken, receiverTitle, receiverBody);

                    receiverLogId = await notificationRepository.LogInformation(
                        request.ReferenceCode ?? request.ReceiverCode,
                        request.AppCode ?? "MB",
                        Code.NotificationTypeCode.FIREBASE,
                        template.TemplateID,
                        request.Redirect,
                        JsonConvert.SerializeObject(receiverDict),
                        notificationCategory: request.NotificationCategory
                    );

                    if (receiverLogId <= 0)
                    {
                        Console.WriteLine("⚠️ Receiver log failed: " + request.ReceiverCode);
                        return false;
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(
                "🔥 Exception in GenereateFirebaseNotificationAsync: " + ex.Message
            );
            return false;
        }
    }

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
            var response = await firebaseMessaging.SendAsync(message);
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

        await pushNotificationLogRepository.InsertAsync(log);
    }

    private static string ReplaceTokens(string template, object data)
    {
        if (data is string s)
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
        }

        var dict =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(
                JsonConvert.SerializeObject(data)
            ) ?? [];
        foreach (var kv in dict)
        {
            template = template.Replace($"{{{kv.Key}}}", kv.Value?.ToString() ?? "");
        }
        return template;
    }
}
