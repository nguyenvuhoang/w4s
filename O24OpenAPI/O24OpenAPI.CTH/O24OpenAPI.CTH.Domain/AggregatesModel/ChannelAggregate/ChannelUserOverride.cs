using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public partial class ChannelUserOverride : BaseEntity
{
    public string? ChannelIdRef { get; set; }
    public string UserCode { get; set; } = default!;
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsAllowedAllDay { get; set; } = true;
    public bool AllowWhenDisabled { get; set; } = false;
    public string? Note { get; set; }
}
