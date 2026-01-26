using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.CTH.API.Application.Models.Channel;

public class ChannelIntervalVm
{
    public string Start { get; set; } = default!; // "07:00"
    public string End { get; set; } = default!; // "19:00"
}

public class ChannelDayVm
{
    public int DayOfWeek { get; set; } // 0=Sun..6=Sat
    public string DayName { get; set; } = default!;
    public bool IsClosed { get; set; }
    public List<ChannelIntervalVm> Intervals { get; set; } = [];
    public bool IsToday { get; set; }
}

public class ChannelVm : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public string ChannelId { get; set; } = default!;
    public string ChannelName { get; set; } = default!;
    public string Description { get; set; }
    public bool Status { get; set; }
    public bool IsAlwaysOpen { get; set; }
    public string TimeZoneId { get; set; } = "Asia/Ho_Chi_Minh";
    public List<ChannelDayVm> Weekly { get; set; } = [];
    public bool IsOpenNow { get; set; }
}
