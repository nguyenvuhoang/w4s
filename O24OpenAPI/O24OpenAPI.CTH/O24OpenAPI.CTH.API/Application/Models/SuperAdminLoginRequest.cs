using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

/// <summary>
/// The super admin login request class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class SuperAdminLoginRequest : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the user name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }
}
