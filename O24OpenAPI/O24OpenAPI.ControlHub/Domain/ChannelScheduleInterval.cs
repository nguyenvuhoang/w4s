using LinqToDB;
using LinqToDB.Mapping;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

public partial class ChannelScheduleInterval : BaseEntity
{
    public int ChannelScheduleIdRef { get; set; }   // FK -> ChannelSchedule
    [Column(DataType = DataType.Time)]
    public TimeSpan StartTime { get; set; }         // vd 08:00
    [Column(DataType = DataType.Time)]
    public TimeSpan EndTime { get; set; }           // vd 17:00 (nếu End < Start => qua đêm)
    public int SortOrder { get; set; } = 0;

}
