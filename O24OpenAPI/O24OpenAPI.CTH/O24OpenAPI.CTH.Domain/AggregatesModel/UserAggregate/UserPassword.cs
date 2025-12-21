using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The user password class
/// </summary>
/// /// <seealso cref="BaseEntity"/>
public class UserPassword : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the value of the salt
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// Gets or sets the value of the updated on utc
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
