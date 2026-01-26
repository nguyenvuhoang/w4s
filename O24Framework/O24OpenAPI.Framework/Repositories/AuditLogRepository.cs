using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data;

namespace O24OpenAPI.Framework.Repositories;

public interface IEntityAuditRepository : IRepository<EntityAudit>
{
    Task<List<EntityAudit>> GetByExecutionIdAsync(
        string executionId,
        CancellationToken cancellationToken = default
    );

    Task ClearOldAudits();
}

[RegisterService(Lifetime.Scoped)]
public class EntityAuditRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<EntityAudit>(dataProvider, staticCacheManager), IEntityAuditRepository
{
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;

    public async Task ClearOldAudits()
    {
        await DeleteWhere(s => s.CreatedOnUtc.Value.Date < DateTime.UtcNow.Date);
    }

    public async Task<List<EntityAudit>> GetByExecutionIdAsync(
        string executionId,
        CancellationToken cancellationToken = default
    )
    {
        var query = await Table
            .Where(al => al.ExecutionId == executionId)
            .ToListAsync(cancellationToken);
        return query;
    }
}
