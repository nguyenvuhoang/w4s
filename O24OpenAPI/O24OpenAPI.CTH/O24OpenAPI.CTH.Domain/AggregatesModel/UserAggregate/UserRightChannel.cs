using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

/// <summary>
/// The user right class
/// </summary>
/// <seealso cref="BaseEntity"/>
[Auditable]
public partial class UserRightChannel : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string? ChannelId { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the invoke
    /// </summary>
    public bool Invoke { get; set; }

    /// <summary>
    /// Gets or sets the value of the updated on utc
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
