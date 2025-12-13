namespace O24OpenAPI.Web.CMS.Domain;

public class CoreApiKeys : BaseEntity
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string BICCode { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Environment { get; set; }
    public string? Scopes { get; set; }
    public DateTime ExpiredOnUtc { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string? Status { get; set; } = "ACTIVE";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? LastUsedOnUtc { get; set; }
    public int UsageCount { get; set; } = 0;
    public int AccessTokenTtlSeconds { get; set; } = 2_592_000;
    public int AccessTokenMaxTtlSeconds { get; set; } = 2_592_000;
    public int? AccessTokenMaxUses { get; set; }
    public string? AccessTokenTrustedIPs { get; set; } = string.Empty;
    public string? ClientSecretTrustedIPs { get; set; } = string.Empty;
    public string? ClientSecretDescription { get; set; } = string.Empty;
    public DateTime? ClientSecretExpiresOnUtc { get; set; }

    public CoreApiKeys Clone()
    {
        return (CoreApiKeys)this.MemberwiseClone();
    }
}
