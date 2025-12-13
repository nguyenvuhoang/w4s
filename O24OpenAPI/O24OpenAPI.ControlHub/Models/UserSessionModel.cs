using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The user session model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class UserSessionModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the value of the refresh token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the value of the refresh token expiration time
    /// </summary>
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel roles
    /// </summary>
    public HashSet<string> ChannelRoles { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the value of the device
    /// </summary>
    public string Device { get; set; }
    public string BranchCode { get; set; }
    public string UserCode { get; set; }
    public string UserName { get; set; }

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
    public string ChannelId { get; set; }

    /// <summary>
    /// Ises the valid
    /// </summary>
    /// <returns>The bool</returns>
    public bool IsValid()
    {
        return !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }

    /// <summary>
    /// Ises the valid
    /// </summary>
    /// <returns>The bool</returns>
    public bool IsRefreshTokenValid()
    {
        return !IsRevoked && RefreshTokenExpiresAt > DateTime.UtcNow;
    }

    /// <summary>
    /// Revokes this instance
    /// </summary>
    public void Revoke()
    {
        IsRevoked = true;
    }

    /// <summary>
    /// Clones this instance
    /// </summary>
    /// <returns>The user session</returns>
    public UserSessionModel Clone()
    {
        return (UserSessionModel)MemberwiseClone();
    }
}
