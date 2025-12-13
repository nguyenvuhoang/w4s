using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class UserNotifications : BaseEntity
{
    public int NotificationID { get; set; }
    public string UserCode { get; set; }
    public DateTime DateTime { get; set; }
    public string PushId { get; set; }
    public string UserDevice { get; set; }
    public string PhoneNumber { get; set; }
}

