using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class Notification : BaseEntity
{
    public string UserCode { get; set; } = null!;
    public string AppType { get; set; } = null!;
    public string NotificationType { get; set; }
    public string NotificationCategory { get; set; } = string.Empty;
    public string DataValue { get; set; }
    public string TemplateID { get; set; } = null!;
    public string Redirect { get; set; }
    public bool IsRead { get; set; }
    public bool IsPushed { get; set; }
    public DateTime DateTime { get; set; }
    public bool? IsProcessed { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
