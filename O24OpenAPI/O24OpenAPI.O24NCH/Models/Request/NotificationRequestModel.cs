using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class NotificationRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string NotificationType { get; set; }
    public string Email { get; set; }
    public Dictionary<string, object> SenderData { get; set; }
    public Dictionary<string, object> DataTemplate { get; set; } = [];
    public List<string> AttachmentBase64Strings { get; set; } = [];
    public List<string> AttachmentFilenames { get; set; } = [];
    public List<MimeEntity> MimeEntities { get; set; } = [];
    public List<int> FileIds { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}
