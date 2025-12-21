using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public class ChannelOverrideInterval : BaseEntity
{
    public int ChannelOverrideIdRef { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SortOrder { get; set; } = 0;
}
