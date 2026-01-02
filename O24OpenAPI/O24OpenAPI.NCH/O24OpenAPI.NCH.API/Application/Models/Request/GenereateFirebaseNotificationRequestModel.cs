namespace O24OpenAPI.NCH.API.Application.Models.Request;

public class FirebaseNotificationRequestModel : PushNotificationModel
{
    public Dictionary<string, object> ReceiverData { get; set; } = [];
    public Dictionary<string, object> SenderData { get; set; }
}
