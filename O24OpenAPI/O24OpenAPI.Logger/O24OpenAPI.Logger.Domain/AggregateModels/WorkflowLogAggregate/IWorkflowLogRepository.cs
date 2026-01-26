using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

public interface IWorkflowLogRepository : IRepository<WorkflowLog>
{
    Task<WorkflowLog?> GetByExecutionIdAsync(string executionId);
}
