using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models;

namespace O24OpenAPI.NCH.API.Application.Models.Request;

public class SMSProviderCreateModel : BaseTransactionModel
{
    public SMSProviderCreateModel() { }

    public string CountryPrefix { get; set; }
    public string AllowedPrefix { get; set; }
    public string ApiUsername { get; set; }
    public string ApiPassword { get; set; }
    public string ApiKey { get; set; }
    public string BrandName { get; set; }
    public bool? IsActive { get; set; }
    public string ProviderName { get; set; }
    public List<SMSProviderConfigModel> SMSProviderConfig { get; set; }
}
