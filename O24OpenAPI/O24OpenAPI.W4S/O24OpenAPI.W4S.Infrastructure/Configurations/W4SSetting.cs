using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.W4S.Infrastructure.Configurations;

public class W4SSetting : ISettings
{
    public string BaseCurrency { get; set; } = "VND";
    public string DefaultWalletIcon { get; set; } = "wallet";
    public string DefaultWalletColor { get; set; } = "#2e4eb4";
}
