using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.PMT.Infrastructure.Configurations;

public class VNPayConfiguration : IConfig
{
    public string TmnCode { get; set; } = string.Empty;
    public string HashSecret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public string IpnUrl { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string VnpApi { get; set; } = string.Empty;
}
