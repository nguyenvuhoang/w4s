using LinKit.Core.Cqrs;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

[CqrsHandler]
public class CreateWorkflowLog(IWorkflowLogRepository workflowLogRepository)
    : ICommandHandler<CreateWorkflowLogCommand>
{
    public async Task<Unit> HandleAsync(
        CreateWorkflowLogCommand request,
        CancellationToken cancellationToken = default
    )
    {
        WorkflowLog log = request.ToWorkflowLog();
        await workflowLogRepository.InsertAsync(log);
        return Unit.Value;
    }
}
