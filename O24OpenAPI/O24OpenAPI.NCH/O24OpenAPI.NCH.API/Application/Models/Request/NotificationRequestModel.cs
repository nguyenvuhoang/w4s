using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.Request;

namespace O24OpenAPI.NCH.Models.Request;

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
    public List<O24MimeEntity> MimeEntities { get; set; } = [];
    public List<int> FileIds { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}
