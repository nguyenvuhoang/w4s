using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public class ChannelOverride : BaseEntity
{
    public int ChannelIdRef { get; set; }
    public DateOnly Date { get; set; }

    /// <summary>Nếu true: đóng cả ngày. Nếu false: dùng Intervals bên dưới.</summary>
    public bool IsClosedAllDay { get; set; } = false;

    /// <summary>Ưu tiên cao hơn ChannelSchedule.</summary>
    public virtual ICollection<ChannelOverrideInterval> Intervals { get; set; } = new List<ChannelOverrideInterval>();
}
