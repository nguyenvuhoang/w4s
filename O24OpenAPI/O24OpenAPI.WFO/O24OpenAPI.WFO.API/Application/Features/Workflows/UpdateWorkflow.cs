using LinKit.Core.Abstractions;
using Newtonsoft.Json;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Diagnostics;

namespace O24OpenAPI.WFO.API.Application.Features.Workflows;

public interface IUpdateWorkflowHandler
{
    Task<WorkflowResponse> UpdateWorkflow(WorkflowInput input);
}

[RegisterService(Lifetime.Scoped)]
public class UpdateWorkflowHandler(
    IWorkflowDefRepository workflowDefRepository,
    IWorkflowStepRepository workflowStepRepository
) : IUpdateWorkflowHandler
{
    public async Task<WorkflowResponse> UpdateWorkflow(WorkflowInput input)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        WorkflowResponse response = new()
        {
            execution_id = input.ExecutionId,
            transaction_date = input.TransactionDate,
        };
        try
        {
            if (!input.Fields.TryGetValue("def", out object defObject))
            {
                throw new InvalidOperationException("Invalid workflow definition");
            }
            if (!input.Fields.TryGetValue("steps", out object stepsObject))
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            WorkflowDef wfDef =
                JsonConvert.DeserializeObject<WorkflowDef>(defObject.ToSerialize())
                ?? throw new InvalidOperationException("Invalid workflow definition");
            WorkflowDef exit =
                await workflowDefRepository.GetByWorkflowIdAndChannelIdAsync(wfDef.WorkflowId, wfDef.ChannelId)
                ?? throw new InvalidOperationException(
                    $"Workflow [{wfDef.WorkflowId}] does not exit."
                );

            await workflowDefRepository.Delete(exit);
            wfDef = await workflowDefRepository.InsertAsync(wfDef);
            List<WorkflowStep> wfSteps = JsonConvert.DeserializeObject<List<WorkflowStep>>(
                stepsObject.ToSerialize()
            );
            if (wfSteps == null || wfSteps.Count == 0)
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            await workflowStepRepository.DeleteByWorkflowIdAsync(wfDef.WorkflowId);

            foreach (WorkflowStep step in wfSteps)
            {
                step.WorkflowId = wfDef.WorkflowId;
                await workflowStepRepository.InsertAsync(step);
            }
            stopwatch.Stop();

            response.data["data"] = true;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = WorkflowExecutionStatus.Completed.ToString();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            response.data["data"] = false;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = WorkflowExecutionStatus.Error.ToString();
            response.error_message = ex.Message;
            response.error_code = ex.HResult.ToString("X8");
        }
        return response;
    }
}
