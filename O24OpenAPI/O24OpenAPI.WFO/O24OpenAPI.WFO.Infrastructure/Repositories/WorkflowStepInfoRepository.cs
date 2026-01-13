using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WorkflowStepInfoRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WorkflowStepInfo>(dataProvider, staticCacheManager),
        IWorkflowStepInfoRepository
{
    public async Task<List<WorkflowStepInfo>> GetByExecutionId(string executionId)
    {
        return await Table
            .Where(s => s.execution_id == executionId)
            .OrderBy(s => s.step_order)
            .ToListAsync();
    }

    public async Task<WorkflowStepInfo?> GetByExecutionStep(
        string executionId,
        string stepExecutionId
    )
    {
        WorkflowStepInfo? workflowStepInfo = await Table
            .Where(x => x.execution_id == executionId && x.step_execution_id == stepExecutionId)
            .FirstOrDefaultAsync();
        return workflowStepInfo;
    }
}
