using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class UserDevice : BaseEntity
{
    public string UserCode { get; set; }
    public string DeviceId { get; set; }
    public string DeviceType { get; set; }
    public string ChannelId { get; set; }
    public string Status { get; set; }
    public string PushId { get; set; }
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string OsVersion { get; set; }
    public string AppVersion { get; set; }
    public string DeviceName { get; set; }
    public string Brand { get; set; }
    public bool IsEmulator { get; set; }
    public bool IsRootedOrJailbroken { get; set; }
    public string Network { get; set; }
    public string Memory { get; set; }
    public DateTime LastSeenDateUtc { get; set; } = DateTime.UtcNow;
}
