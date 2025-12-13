using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.Web.Framework.Services;

/// <summary>
/// The entity audit service interface
/// </summary>
public interface IEntityAuditService
{
    /// <summary>
    /// Adds the entity audit
    /// </summary>
    /// <param name="entityAudit">The entity audit</param>
    Task AddAsync(EntityAudit entityAudit);

    /// <summary>
    /// Updates the entity audit
    /// </summary>
    /// <param name="entityAudit">The entity audit</param>
    Task UpdateAsync(EntityAudit entityAudit);

    /// <summary>
    /// Gets the unsent items
    /// </summary>
    /// <returns>A task containing a list of entity audit</returns>
    Task<List<EntityAudit>> GetUnsentItems();
}
