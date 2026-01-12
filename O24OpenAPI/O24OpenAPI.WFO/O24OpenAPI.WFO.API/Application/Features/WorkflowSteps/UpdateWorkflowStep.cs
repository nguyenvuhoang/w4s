using LinKit.Core.Cqrs;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.WFO.API.Application.Models.WorkflowStepModels;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowSteps;

public class UpdateWorkflowStepCommand : ICommand<bool>
{
    [JsonPropertyName("workflow_id")]
    public string WorkflowId { get; set; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    [JsonPropertyName("workflow_step_list")]
    public List<WorkflowStepModel> WorkflowStepList { get; set; }
}

public class UpdateWorkflowStepHandler(
    IWorkflowStepRepository workflowStepRepository,
    IWorkflowDefRepository workflowDefRepository
) : ICommandHandler<UpdateWorkflowStepCommand, bool>
{
    public async Task<bool> HandleAsync(
        UpdateWorkflowStepCommand request,
        CancellationToken cancellationToken = default
    )
    {
        WorkflowDef workflowDef = await workflowDefRepository.GetByWorkflowIdAndChannelIdAsync(
            request.WorkflowId,
            request.ChannelId
        );
        if (workflowDef is null)
        {
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.NotExists,
                request.ChannelId,
                request.WorkflowId
            );
        }
        IPagedList<WorkflowStep> listStep = await workflowStepRepository.GetByWorkflowId(request.WorkflowId);
        foreach (WorkflowStepModel stepNew in request.WorkflowStepList)
        {
            WorkflowStep old = listStep.First(s => s.StepCode == stepNew.StepCode && s.StepOrder == stepNew.StepOrder);
            var stepUpdate = stepNew.ToWorkflowStep(old);
            await workflowStepRepository.Update(stepUpdate);
        }
        return true;
    }
}
