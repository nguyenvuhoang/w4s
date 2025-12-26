using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public partial class ChannelScheduleInterval : BaseEntity
{
    public int ChannelScheduleIdRef { get; set; } // FK -> ChannelSchedule
    public TimeSpan StartTime { get; set; } // vd 08:00
    public TimeSpan EndTime { get; set; } // vd 17:00 (nếu End < Start => qua đêm)
    public int SortOrder { get; set; } = 0;
}
