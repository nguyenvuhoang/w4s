using O24OpenAPI.Core.Enums;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.Framework.Services;

/// <summary>
/// The entity audit service class
/// </summary>
/// <seealso cref="IEntityAuditService"/>
public class EntityAuditService(IRepository<EntityAudit> repo) : IEntityAuditService
{
    /// <summary>
    /// The repo
    /// </summary>
    private readonly IRepository<EntityAudit> _repo = repo;

    /// <summary>
    /// Adds the entity audit
    /// </summary>
    /// <param name="entityAudit">The entity audit</param>
    public async Task AddAsync(EntityAudit entityAudit)
    {
        await _repo.Insert(entityAudit);
    }

    /// <summary>
    /// Updates the entity audit
    /// </summary>
    /// <param name="entityAudit">The entity audit</param>
    public async Task UpdateAsync(EntityAudit entityAudit)
    {
        await _repo.Update(entityAudit);
    }

    /// <summary>
    /// Gets the unsent items
    /// </summary>
    /// <returns>The items</returns>
    public async Task<List<EntityAudit>> GetUnsentItems()
    {
        var items = await _repo
            .Table.Where(x => x.Status != (int)SendEnum.Sent)
            .OrderBy(x => x.CreatedOnUtc)
            .ToListAsync();
        return items;
    }
}
