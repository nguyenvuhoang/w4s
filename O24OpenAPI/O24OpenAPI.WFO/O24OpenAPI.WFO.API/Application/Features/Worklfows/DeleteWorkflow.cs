using LinKit.Core.Abstractions;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Diagnostics;

namespace O24OpenAPI.WFO.API.Application.Features.Worklfows;

public interface IDeleteWorkflowHandler
{
    Task<WorkflowResponse> DeleteWorkflow(WorkflowInput input);
}

[RegisterService(Lifetime.Scoped)]
public class DeleteWorkflowHandler(
    IWorkflowDefRepository workflowDefRepository,
    IWorkflowStepRepository workflowStepRepository
) : IDeleteWorkflowHandler
{
    public async Task<WorkflowResponse> DeleteWorkflow(WorkflowInput input)
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
            if (!input.Fields.TryGetValue("id", out object idOb))
            {
                throw new O24OpenAPIException("Id is required!");
            }
            int id = idOb.ToInt();
            WorkflowDef wfDef =
                await workflowDefRepository.GetById(id)
                ?? throw new InvalidOperationException($"Workflow with id [{id}] does not exit.");
            await workflowDefRepository.Delete(wfDef);
            await workflowStepRepository.DeleteByWorkflowIdAsync(wfDef.WorkflowId);
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
