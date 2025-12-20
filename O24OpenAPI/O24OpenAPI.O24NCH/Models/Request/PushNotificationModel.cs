using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class PushNotificationModel : BaseTransactionModel
{
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
