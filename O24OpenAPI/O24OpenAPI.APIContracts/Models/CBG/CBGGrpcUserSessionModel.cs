namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGGrpcUserSessionModel
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? UserId { get; set; }
    public string? LoginName { get; set; }
    public string? Reference { get; set; }
    public string? IpAddress { get; set; }
    public string? Device { get; set; }
    public DateTime? Workingdate { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string? RevokeReason { get; set; }
    public string? ChannelId { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public string? CommandList { get; set; }
    public string? BranchCode { get; set; }
}
