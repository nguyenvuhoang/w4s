using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Framework.Domain;

/// <summary>
/// The entity audit class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class EntityAudit : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityAudit"/> class
    /// </summary>
    public EntityAudit() { }

    /// <summary>
    /// Gets or sets the value of the entity name
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the value of the action type
    /// </summary>
    public string ActionType { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
