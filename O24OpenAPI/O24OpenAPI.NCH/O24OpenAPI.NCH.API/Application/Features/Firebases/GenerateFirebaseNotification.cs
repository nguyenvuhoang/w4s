using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.NCH.API.Application.Models.Request;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;
using O24OpenAPI.NCH.Domain.Constants;

namespace O24OpenAPI.NCH.API.Application.Features.Firebases;

public class FirebaseNotificationRequestModel : PushNotificationModel, ICommand<bool>
{
    public Dictionary<string, object> ReceiverData { get; set; } = [];
    public Dictionary<string, object> SenderData { get; set; }
}

[CqrsHandler]
public class GenerateFirebaseNotificationHandler(
    INotificationTemplateRepository notificationTemplateRepository,
    ICTHGrpcClientService cTHGrpcClientService,
    IMediator mediator,
    INotificationRepository notificationRepository
) : ICommandHandler<FirebaseNotificationRequestModel, bool>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_PUSH_FIREBASE_NOTIFICATION)]
    public async Task<bool> HandleAsync(
        FirebaseNotificationRequestModel request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            int receiverLogId = -1;

            NotificationTemplate template =
                await notificationTemplateRepository.GetByTemplateIdAsync(request.TemplateID);
            if (template == null)
            {
                Console.WriteLine("⚠️ Template not found: " + request.TemplateID);
                return false;
            }

            Dictionary<string, string> titleDict =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Title ?? "{}")
                ?? [];
            Dictionary<string, string> bodyDict =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Body ?? "{}")
                ?? [];

            string lang = string.IsNullOrWhiteSpace(request.Language) ? "en" : request.Language;
            string rawTitle = titleDict.TryGetValue(lang, out string valuetTitle)
                ? valuetTitle
                : titleDict.GetValueOrDefault("en") ?? "";
            string rawBody = bodyDict.TryGetValue(lang, out string valueBody)
                ? valueBody
                : bodyDict.GetValueOrDefault("en") ?? "";

            Dictionary<string, string> senderDict =
                request.SenderData?.ToDictionary(
                    k => k.Key,
                    dv => dv.Value?.ToString() ?? string.Empty
                ) ?? [];

            string senderTitle = ReplaceTokens(rawTitle, senderDict);
            string senderBody = ReplaceTokens(rawBody, senderDict);

            string senderToken =
                request.SenderPushId
                ?? await cTHGrpcClientService.GetUserPushIdAsync(request.UserCode);
            if (string.IsNullOrWhiteSpace(senderToken))
            {
                Console.WriteLine("⚠️ Sender token not found for user: " + request.UserCode);
                return false;
            }

            await mediator.SendAsync(
                new FirebaseSendNotificationCommand
                {
                    Token = senderToken,
                    Tittle = senderTitle,
                    Body = senderBody,
                },
                cancellationToken
            );

            int senderLogId = await notificationRepository.LogInformation(
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
                string receiverToken = await cTHGrpcClientService.GetUserPushIdAsync(
                    request.ReceiverCode
                );
                if (!string.IsNullOrWhiteSpace(receiverToken))
                {
                    Dictionary<string, string> receiverDict =
                        request.ReceiverData != null
                            ? request.ReceiverData.ToDictionary(
                                kv => kv.Key,
                                kv => kv.Value?.ToString() ?? string.Empty
                            )
                            : [];

                    string receiverTitle = ReplaceTokens(rawTitle, receiverDict);
                    string receiverBody = ReplaceTokens(rawBody, receiverDict);

                    await mediator.SendAsync(
                        new FirebaseSendNotificationCommand
                        {
                            Token = receiverToken,
                            Tittle = receiverTitle,
                            Body = receiverBody,
                        },
                        cancellationToken
                    );

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

    private static string ReplaceTokens(string template, object data)
    {
        if (data is string s)
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
        }

        Dictionary<string, object> dict =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(
                JsonConvert.SerializeObject(data)
            ) ?? [];
        foreach (KeyValuePair<string, object> kv in dict)
        {
            template = template.Replace($"{{{kv.Key}}}", kv.Value?.ToString() ?? "");
        }
        return template;
    }
}
