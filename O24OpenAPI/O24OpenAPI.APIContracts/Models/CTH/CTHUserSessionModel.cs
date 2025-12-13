namespace O24OpenAPI.APIContracts.Models.CTH;

public class CTHUserSessionModel
{
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the value of the refresh token
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the value of the refresh token expiration time
    /// </summary>
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel roles
    /// </summary>
    public string? ChannelRoles { get; set; }

    public string? BranchCode { get; set; }
    public string? UserCode { get; set; }
    public string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the value of the device
    /// </summary>
    public string? Device { get; set; }

    /// <summary>
    /// Gets or sets the value of the expires at
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the value of the is revoked
    /// </summary>
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string? ChannelId { get; set; }

    public string? SignatureKey { get; set; }
}
