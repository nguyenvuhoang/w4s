using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public partial class NotificationDetail : BaseEntity
{
    public string AppType { get; set; } = null!;
    public string? NotificationType { get; set; }
    public string? NotificationCategory { get; set; }
    public string? Description { get; set; }
    public string? TargetType { get; set; }
    public int GroupID { get; set; }
    public string? UserCode { get; set; }
    public string? Body { get; set; }
    public bool IsPushed { get; set; }
    public DateTime DateTime { get; set; }
    public bool Status { get; set; }
}
