using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public partial class PushNotificationLog : BaseEntity
{
    public string Token { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Data { get; set; }
    public string RequestMessage { get; set; }
    public string ResponseId { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}
