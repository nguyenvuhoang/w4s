namespace O24OpenAPI.ControlHub.Models.User;

using O24OpenAPI.Web.Framework.Models;

/// <summary>
/// Defines the <see cref="UserInfoModel" />
/// </summary>
public class UserInfoModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the UserId
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the UserCode
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the LoginName
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Gets or sets the FullName
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the PhoneNumber
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the ChannelId
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the UserDeviceId
    /// </summary>
    public string UserDeviceId { get; set; }

    /// <summary>
    /// Gets or sets the UserPushId
    /// </summary>
    public string UserPushId { get; set; }
}
