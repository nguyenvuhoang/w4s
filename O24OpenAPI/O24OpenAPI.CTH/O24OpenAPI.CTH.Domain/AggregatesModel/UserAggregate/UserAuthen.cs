using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The user password class
/// </summary>
/// /// <seealso cref="BaseEntity"/>
public class UserAuthen : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }
    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string AuthenType { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the Key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the value of the SmartOTP
    /// </summary>
    public string SmartOTP { get; set; }

    /// <summary>
    /// Gets or sets the value of the PinCode
    /// </summary>
    public string PinCode { get; set; }
    /// <summary>
    /// Gets or sets the value of the SMSOTP
    /// </summary>
    public string SMSOTP { get; set; }
    /// <summary>
    /// Gets or sets the value of the Phone
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool? IsActive { get; set; }
}
