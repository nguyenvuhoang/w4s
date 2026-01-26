using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WorkflowInfoRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WorkflowInfo>(dataProvider, staticCacheManager), IWorkflowInfoRepository
{
    public async Task<WorkflowInfo?> GetByExecutionIdAsync(string executionId)
    {
        return await Table.Where(s => s.execution_id == executionId).FirstOrDefaultAsync();
    }
}
