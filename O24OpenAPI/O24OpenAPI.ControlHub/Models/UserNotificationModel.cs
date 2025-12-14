namespace O24OpenAPI.ControlHub.Models;

using O24OpenAPI.Web.Framework.Models;

/// <summary>
/// Defines the <see cref="UserNotificationModel" />
/// </summary>
public class UserNotificationModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the UserCode
    /// </summary>
    public string UserCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the NotificationId
    /// </summary>
    public string NotificationId { get; set; } = string.Empty;
}
