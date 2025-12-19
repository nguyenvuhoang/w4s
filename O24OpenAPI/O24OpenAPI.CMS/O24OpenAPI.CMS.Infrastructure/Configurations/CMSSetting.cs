using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.CMS.Infrastructure.Configurations;

public class CMSSetting : ISettings
{
    public string? SecretKey { get; set; }
    public string? HostPort { get; set; }
    public string? ClientRsaPublicKey { get; set; }
    public string? TemplateHostDev { get; set; }
    public bool IsDev { get; set; }
    public string? FirebaseConfigPath { get; set; }
    public double NotificationExpiredInSeconds { get; set; } = 120;
    public string? DefaultLanguage { get; set; } = "en";
    public string? CoreMode { get; set; }
    public string? BaseCurrency { get; set; } = "LAK";
    public string? ListF8Transaction { get; set; }
    public HashSet<string> ListWorkflowNotCheckSession { get; set; } = ["SYS_CREATE_SADMIN"];
    public HashSet<string>? ListChannelCheckSignature { get; set; }
    public HashSet<string> ListWorkflowNotCheckSignature { get; set; } =
        ["CMS_REGISTER_SIGNATURE_KEY", "UMG_LOGIN"];
    public string? CMSURL { get; set; } = string.Empty;
}
