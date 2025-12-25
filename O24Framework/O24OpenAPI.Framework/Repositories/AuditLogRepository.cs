using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.Framework.Repositories;

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<List<AuditLog>> GetByExecutionIdAsync(
        string executionId,
        CancellationToken cancellationToken = default
    );
}

[RegisterService(Lifetime.Scoped)]
public class AuditLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<AuditLog>(dataProvider, staticCacheManager), IAuditLogRepository
{
    public async Task<List<AuditLog>> GetByExecutionIdAsync(
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
