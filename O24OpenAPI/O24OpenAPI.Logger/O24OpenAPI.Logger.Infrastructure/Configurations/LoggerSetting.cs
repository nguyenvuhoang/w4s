using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Logger.Infrastructure.Configurations;

public class LoggerSetting : ISettings
{
    public int LogRetentionDays { get; set; } = 7;
}
