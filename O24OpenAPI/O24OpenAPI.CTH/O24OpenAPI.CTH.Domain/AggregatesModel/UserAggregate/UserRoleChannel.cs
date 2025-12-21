using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The user role class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class UserRoleChannel : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the value of the role name
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Gets or sets the value of the role description
    /// </summary>
    public string RoleDescription { get; set; }

    /// <summary>
    /// Gets or sets the value of the user created
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    /// Gets or sets the value of the user modified
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the updated on utc
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
