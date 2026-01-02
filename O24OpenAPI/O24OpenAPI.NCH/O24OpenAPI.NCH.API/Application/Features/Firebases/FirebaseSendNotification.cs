using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Firebases;

public class FirebaseSendNotificationCommand : ICommand
{
    public string Token { get; set; }
    public string Tittle { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Data { get; set; }
}

public class FirebaseSendNotificationHandler(
    FirebaseAdmin.Messaging.FirebaseMessaging firebaseMessaging,
    IPushNotificationLogRepository pushNotificationLogRepository
) : ICommandHandler<FirebaseSendNotificationCommand>
{
    public async Task<Unit> HandleAsync(
        FirebaseSendNotificationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        Dictionary<string, string> data = request.Data;
        string body = request.Body;
        string token = request.Token;
        string tittle = request.Tittle;

        data ??= [];
        data["title"] = tittle;
        data["body"] = body;

        var log = new PushNotificationLog
        {
            Token = token,
            Title = tittle,
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
                    Title = tittle,
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
            string response = await firebaseMessaging.SendAsync(message);
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

        return Unit.Value;
    }
}
