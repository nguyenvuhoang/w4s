using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public partial class ChannelSchedule : BaseEntity
{
    public int ChannelIdRef { get; set; }          // FK -> Channel
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>Khoảng hiệu lực (nullable = vĩnh viễn).</summary>
    public DateOnly? EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }

    /// <summary>Nếu true thì ngày này nghỉ, bỏ qua Intervals.</summary>
    public bool IsClosed { get; set; } = false;

    public virtual ICollection<ChannelScheduleInterval> Intervals { get; set; } = [];
}
