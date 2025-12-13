using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

/// <summary>
/// The user right model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class UserRightModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>

    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the invoke
    /// </summary>
    public bool Invoke { get; set; }
    /// <summary>
    /// /// Gets or sets the value of the approve
    /// </summary>
    public bool Approve { get; set; }
    /// <summary>
    /// /// Gets or sets the value of the CommandId
    /// </summary>
    public string CommandId { get; set; }
    /// <summary>
    /// /// Gets or sets the value of the CommandIdDetail
    /// </summary>
    public string CommandIdDetail { get; set; } = "A";
}
