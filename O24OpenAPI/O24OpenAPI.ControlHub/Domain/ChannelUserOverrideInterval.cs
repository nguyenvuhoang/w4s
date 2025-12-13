using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

public class ChannelUserOverrideInterval : BaseEntity
{
    public int ChannelUserOverrideIdRef { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SortOrder { get; set; }
}
