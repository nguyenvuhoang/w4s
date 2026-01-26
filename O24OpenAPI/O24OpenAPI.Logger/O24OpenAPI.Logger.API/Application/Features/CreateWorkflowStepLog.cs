using LinKit.Core.Cqrs;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

[CqrsHandler]
public class CreateWorkflowStepLog(IWorkflowStepLogRepository workflowStepLogRepository)
    : ICommandHandler<CreateWorkflowStepLogCommand>
{
    public async Task<Unit> HandleAsync(
        CreateWorkflowStepLogCommand request,
        CancellationToken cancellationToken = default
    )
    {
        WorkflowStepLog log = request.ToWorkflowStepLog();
        await workflowStepLogRepository.InsertAsync(log);
        return Unit.Value;
    }
}
