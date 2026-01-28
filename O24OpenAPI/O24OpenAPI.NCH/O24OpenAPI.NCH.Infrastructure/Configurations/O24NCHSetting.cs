using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.NCH.Infrastructure.Configurations;

public class O24NCHSetting : ISettings
{
    public string? FirebaseConfigPath { get; set; }
    public double NotificationExpiredInSeconds { get; set; } = 120;
    public string IconPhone { get; set; } = string.Empty;
    public string IconWebsite { get; set; } = string.Empty;
    public string LogoBankFooter { get; set; } = string.Empty;
    public string LogoBankHeader { get; set; } = string.Empty;
    public string TelegramBotToken { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = "en";
    public string FirebaseProjectId { get; set; } = string.Empty;
    public string ZnsSendEndpoint { get; set; } = string.Empty;
    public string ZnsCreateTemplateEndpoint { get; set; } = string.Empty;
}
