using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Features.Workflows;

[ApiEndpoint(ApiMethod.Post, "workflows/clone-for-channel")]
public class CloneWorkflowForChannelCommand : ICommand<bool>
{
    public string WorkflowId { get; set; }
    public string CurrentChannelId { get; set; }
    public string ChannelId { get; set; }
}

[CqrsHandler]
public class CloneWorkflowForChannelCommandHandler(
    IWorkflowDefRepository workflowRepository,
    IWorkflowStepRepository workflowAggregateRepository
) : ICommandHandler<CloneWorkflowForChannelCommand, bool>
{
    public async Task<bool> HandleAsync(
        CloneWorkflowForChannelCommand request,
        CancellationToken cancellationToken = default
    )
    {
        WorkflowDef workflowDef = await workflowRepository.GetByWorkflowIdAndChannelIdAsync(
            request.WorkflowId,
            request.CurrentChannelId
        );
        if (workflowDef == null)
        {
            throw new InvalidOperationException(
                $"Workflow definition not found for WorkflowId: {request.WorkflowId}, ChannelId: {request.CurrentChannelId}"
            );
        }
        List<WorkflowStep> wfSteps = await workflowAggregateRepository.GetByWorkflowIdAsync(request.WorkflowId);
        WorkflowDef newWorkflowDef = workflowDef.ToWorkflowDef();
        newWorkflowDef.WorkflowId = workflowDef.WorkflowId.Replace(
            workflowDef.ChannelId,
            request.ChannelId
        );
        newWorkflowDef.ChannelId = request.ChannelId;
        await workflowRepository.InsertAsync(newWorkflowDef);
        foreach (WorkflowStep step in wfSteps)
        {
            WorkflowStep newStep = step.ToWorkflowStep();
            newStep.WorkflowId = newWorkflowDef.WorkflowId;
            await workflowAggregateRepository.InsertAsync(newStep);
        }
        return true;
    }
}
