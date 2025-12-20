using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The change password 24 open api request model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class ChangePasswordO24OpenAPIRequestModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the LoginName
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the value of the oldpassword
    /// </summary>
    public string NewPassword { get; set; }
}
