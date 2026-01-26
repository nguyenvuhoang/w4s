using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

public interface IWorkflowDefRepository : IRepository<WorkflowDef>
{
    Task<WorkflowDef?> GetByWorkflowIdAndChannelIdAsync(string workflowId, string channelId);
}
