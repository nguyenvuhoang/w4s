using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Response;

public class SMSProviderResponse : BaseO24OpenAPIModel
{
    public string ProviderName { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string CountryPrefix { get; set; } = string.Empty;
    public string AllowedPrefix { get; set; } = string.Empty;
    public string ApiUsername { get; set; } = string.Empty;
    public string ApiPassword { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
