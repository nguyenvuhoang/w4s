using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

/// <summary>
/// The channel class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class Channel : BaseEntity
{
    public string ChannelId { get; set; } = default!;
    public string ChannelName { get; set; } = default!;
    public string Description { get; set; }
    public int SortOrder { get; set; }
    public bool Status { get; set; } = true;

    /// <summary>Mặc định mở 24/7 nếu không có lịch.</summary>
    public bool IsAlwaysOpen { get; set; } = false;

    /// <summary>Múi giờ của kênh (IANA: "Asia/Ho_Chi_Minh").</summary>
    public string TimeZoneId { get; set; } = "Asia/Ho_Chi_Minh";

    public virtual ICollection<ChannelSchedule> Schedules { get; set; } = [];
    public virtual ICollection<ChannelOverride> Overrides { get; set; } = [];
}
