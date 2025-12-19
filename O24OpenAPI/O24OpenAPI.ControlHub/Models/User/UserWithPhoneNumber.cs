namespace O24OpenAPI.ControlHub.Models.User;

using O24OpenAPI.Core.Abstractions;
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
