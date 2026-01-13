using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public interface IWorkflowStepInfoRepository : IRepository<WorkflowStepInfo>
{
    Task<WorkflowStepInfo?> GetByExecutionStep(string executionId, string stepExecutionId);

    Task<List<WorkflowStepInfo>> GetByExecutionId(string executionId);
}
