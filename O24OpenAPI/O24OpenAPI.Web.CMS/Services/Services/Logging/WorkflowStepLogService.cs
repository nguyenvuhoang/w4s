using LinqToDB;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using static O24OpenAPI.Client.Scheme.Workflow.WorkflowScheme.RESPONSE;

namespace O24OpenAPI.Web.CMS.Services.Services.Logging;

public class WorkflowStepLogService(IRepository<WorkflowStepLog> repo) : IWorkflowStepLogService
{
    private readonly IRepository<WorkflowStepLog> _repo = repo;

    public async Task UpdateAsync(WorkflowStepLog log)
    {
        await _repo.Update(log);
    }

    public async Task InsertAsync(WorkflowStepLog log)
    {
        await _repo.Insert(log);
    }

    public async Task LogCallApiNeptune(CallApiModel model, string refid)
    {
        var workflowStepLog = new WorkflowStepLog
        {
            ExecutionId = refid,
            StepExecutionId = refid,
            StepCode = model.WorkflowId,
            Status = EnumResponseStatus.PROCESSING.ToString(),
            WorkflowScheme = model.ToSerialize(),
            RequestData = model.Content.ToSerialize(),
        };
        await InsertAsync(workflowStepLog);
    }

    public async Task UpdateLogNeptune(
        string refid,
        string stepCode,
        string status,
        string responseData
    )
    {
        var workflowStepLog = await _repo
            .Table.Where(s => s.ExecutionId == refid && s.StepCode == stepCode)
            .FirstOrDefaultAsync();
        workflowStepLog.Status = status.ToString();
        workflowStepLog.ResponseData = responseData;
        await UpdateAsync(workflowStepLog);
    }
}
