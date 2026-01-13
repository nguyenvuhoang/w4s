using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public interface IWorkflowInfoRepository : IRepository<WorkflowInfo>
{
    Task<WorkflowInfo?> GetByExecutionIdAsync(string executionId);
}
