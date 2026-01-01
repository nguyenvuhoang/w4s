using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

public interface IWorkflowStepLogRepository : IRepository<WorkflowStepLog>
{
    Task<List<WorkflowStepLog>> GetByExecutionIdAsync(string executionId);
    Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme);
}
