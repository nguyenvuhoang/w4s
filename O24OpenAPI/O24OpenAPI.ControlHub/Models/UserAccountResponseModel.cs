using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ControlHub.Models;

public class UserAccountResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user name
    /// </summary>
    public string UserName { get; set; }
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the value of the roles
    /// </summary>
    public string RoleChannel { get; set; }
    public string UserType { get; set; }
}
