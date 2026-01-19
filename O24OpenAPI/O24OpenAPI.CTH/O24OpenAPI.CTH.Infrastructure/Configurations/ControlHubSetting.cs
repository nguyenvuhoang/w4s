using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.CTH.Infrastructure.Configurations;

public class ControlHubSetting : ISettings
{
    public int MaxFailedAttempts { get; set; } = 5;
    public string? BaseCurrency { get; set; }
    public string? Currdate { get; set; }
}
