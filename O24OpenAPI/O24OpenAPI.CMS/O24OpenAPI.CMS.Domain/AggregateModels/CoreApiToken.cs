namespace O24OpenAPI.CMS.Domain.AggregateModels;

public class CoreApiToken : BaseEntity
{
    public string ClientId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public string Scopes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? LastUsedAt { get; set; }
    public int UsageCount { get; set; } = 0;
    public string BICCD { get; set; } // Bank Identifier Code or Clearing Code
}
