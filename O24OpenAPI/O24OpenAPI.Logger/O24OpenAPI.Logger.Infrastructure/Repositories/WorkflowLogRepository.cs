using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class WorkflowLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WorkflowLog>(dataProvider, staticCacheManager), IWorkflowLogRepository
{
    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<WorkflowLog?> GetByExecutionIdAsync(string executionId)
    {
        WorkflowLog? workflowLog = await Table.FirstOrDefaultAsync(x => x.execution_id == executionId);
        return workflowLog;
    }
}
