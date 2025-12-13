using Newtonsoft.Json;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.O24NCH.Services.Services;

public class FirebaseService(
    INotificationTemplateService notificationTemplateService,
    INotificationService notificationService,
    ICTHGrpcClientService cthGrpcClientService
) : IFirebaseService
{
    private readonly INotificationTemplateService _notificationTemplateService = notificationTemplateService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;
    public async Task<bool> GenereateFirebaseNotificationAsync(FirebaseNotificationRequestModel model)
    {
        try
        {
            int receiverLogId = -1;

            var template = await _notificationTemplateService.GetByTemplateIdAsync(model.TemplateID);
            if (template == null)
            {
                Console.WriteLine("⚠️ Template not found: " + model.TemplateID);
                return false;
            }

            var titleDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Title ?? "{}")
                            ?? [];
            var bodyDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(template.Body ?? "{}")
                            ?? [];

            string lang = string.IsNullOrWhiteSpace(model.Language) ? "en" : model.Language;
            string rawTitle = titleDict.TryGetValue(lang, out string valuetTitle) ? valuetTitle : titleDict.GetValueOrDefault("en") ?? "";
            string rawBody = bodyDict.TryGetValue(lang, out string valueBody) ? valueBody : bodyDict.GetValueOrDefault("en") ?? "";

            var senderDict = model.SenderData?
            .ToDictionary(k => k.Key, dv => dv.Value?.ToString() ?? string.Empty)
            ?? [];

            var senderTitle = ReplaceTokens(rawTitle, senderDict);
            var senderBody = ReplaceTokens(rawBody, senderDict);


            var senderToken = model.SenderPushId ?? await _cthGrpcClientService.GetUserPushIdAsync(model.UserCode);
            if (string.IsNullOrWhiteSpace(senderToken))
            {
                Console.WriteLine("⚠️ Sender token not found for user: " + model.UserCode);
                return false;
            }

            await _notificationService.SendNotificationAsync(senderToken, senderTitle, senderBody);


            var senderLogId = await _notificationService.LogInformation(
                model.UserCode,
                model.AppCode ?? "MB",
                Code.NotificationTypeCode.FIREBASE,
                template.TemplateID,
                model.Redirect,
                JsonConvert.SerializeObject(senderDict),
                notificationCategory: model.NotificationCategory
            );

            if (senderLogId <= 0)
            {
                Console.WriteLine("⚠️ Sender log failed: " + model.UserCode);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(model.ReceiverCode))
            {
                var receiverToken = await _cthGrpcClientService.GetUserPushIdAsync(model.ReceiverCode);
                if (!string.IsNullOrWhiteSpace(receiverToken))
                {
                    var receiverDict = model.ReceiverData != null
                    ? model.ReceiverData.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty)
                    : [];

                    var receiverTitle = ReplaceTokens(rawTitle, receiverDict);
                    var receiverBody = ReplaceTokens(rawBody, receiverDict);

                    await _notificationService.SendNotificationAsync(receiverToken, receiverTitle, receiverBody);

                    receiverLogId = await _notificationService.LogInformation(
                        model.ReferenceCode ?? model.ReceiverCode,
                        model.AppCode ?? "MB",
                        Code.NotificationTypeCode.FIREBASE,
                        template.TemplateID,
                        model.Redirect,
                        JsonConvert.SerializeObject(receiverDict),
                        notificationCategory: model.NotificationCategory
                    );

                    if (receiverLogId <= 0)
                    {
                        Console.WriteLine("⚠️ Receiver log failed: " + model.ReceiverCode);
                        return false;
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("🔥 Exception in GenereateFirebaseNotificationAsync: " + ex.Message);
            return false;
        }
    }


    private static string ReplaceTokens(string template, object data)
    {
        if (data is string s)
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
        }

        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(data)) ?? [];
        foreach (var kv in dict)
        {
            template = template.Replace($"{{{kv.Key}}}", kv.Value?.ToString() ?? "");
        }
        return template;
    }
}
