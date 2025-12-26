using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public partial class SMSProviderConfig : BaseEntity
{
    public string SMSProviderId { get; set; }
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsMainKey { get; set; } = true;
}
