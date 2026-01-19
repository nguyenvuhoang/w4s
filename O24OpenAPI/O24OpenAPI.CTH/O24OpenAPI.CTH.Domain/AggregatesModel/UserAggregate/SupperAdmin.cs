using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The supper admin class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class SupperAdmin : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user name
    /// /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password hash
    /// </summary>
    public string? PasswordHash { get; set; }
}
