namespace O24OpenAPI.Web.CMS.Models.OpenAPI;

public class CreateOpenAPIRequestModel : BaseTransactionModel
{
    public string ClientId { get; set; }
    public string DisplayName { get; set; }
    public string Environment { get; set; }
    public string Scopes { get; set; }
    public string BICCode { get; set; }
    public DateTime ExpiredOnUtc { get; set; }
    public bool IsActive { get; set; }
    public int AccessTokenTtlSeconds { get; set; }
    public int AccessTokenMaxTtlSeconds { get; set; }
    public int AccessTokenMaxUses { get; set; }
    public string AccessTokenTrustedIPs { get; set; }
    public string ClientSecretTrustedIPs { get; set; }
    public string ClientSecretDescription { get; set; }
    public DateTime ClientSecretExpiresOnUtc { get; set; }
}
