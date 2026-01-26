namespace O24OpenAPI.CTH.API.Application.Models.User;

using O24OpenAPI.Framework.Models;

/// <summary>
/// Defines the <see cref="UserWithPhoneNumber" />
/// </summary>
public class UserWithPhoneNumber : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the PhoneNumber
    /// </summary>
    public string PhoneNumber { get; set; }
}
