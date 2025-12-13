using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The change password 24 open api request model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class VerifyUserRequestModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the LoginName
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// Gets or sets the value of the oldpassword
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Get or sets the value of the newpassword
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

}
