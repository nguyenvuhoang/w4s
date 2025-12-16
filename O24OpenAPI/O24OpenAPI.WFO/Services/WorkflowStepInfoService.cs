using LinqToDB;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Services;

public class WorkflowStepInfoService(IRepository<WorkflowStepInfo> Repo) : IWorkflowStepInfoService
{
    private readonly IRepository<WorkflowStepInfo> _repo = Repo;

    public async Task AddAsync(WorkflowStepInfo workflowStepInfo)
    {
        await _repo.Insert(workflowStepInfo);
    }

    public async Task BulkAddAsync(List<WorkflowStepInfo> workflowStepInfos)
    {
        await _repo.BulkInsert(workflowStepInfos);
    }

    public async Task UpdateAsync(WorkflowStepInfo workflowStepInfo)
    {
        await _repo.Update(workflowStepInfo);
    }

    public async Task<WorkflowStepInfo> GetByExecutionStep(
        string executionId,
        string stepExecutionId
    )
    {
        var workflowStepInfo = await _repo
            .Table.Where(x =>
                x.execution_id == executionId && x.step_execution_id == stepExecutionId
            )
            .FirstOrDefaultAsync();
        return workflowStepInfo;
    }

    public async Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme)
    {
        try
        {
            var header = wFScheme.request.request_header;
            var response = wFScheme.response;
            var isSuccess = response.IsSuccess();

            var stepInfo = await GetByExecutionStep(header.execution_id, header.step_execution_id);

            var content = isSuccess
                ? response.data.ToSerialize()
                : new Dictionary<string, object>
                {
                    { "error", response.error_message },
                }.ToSerialize();

            var status = isSuccess ? (int)Enum_STATUS.Completed : (int)Enum_STATUS.Error;

            if (stepInfo != null)
            {
                stepInfo.p1_status = status;
                stepInfo.p1_error = response.error_code;
                stepInfo.p1_content = content;

                await UpdateAsync(stepInfo);
            }
            else
            {
                stepInfo = new WorkflowStepInfo
                {
                    step_execution_id = header.step_execution_id,
                    execution_id = header.execution_id,
                    step_order = header.step_order,
                    step_code = header.step_code,
                    p1_request = wFScheme.request.ToSerialize(),
                    p1_start = header.utc_send_time,
                    p1_status = status,
                    p1_error = response.error_code,
                    p1_content = content,
                };

                await AddAsync(stepInfo);
            }
        }
        catch (Exception ex)
        {
            var logService = EngineContext.Current.Resolve<ILogger>();
            logService.LogError(ex.Message, ex, null, wFScheme.request.request_header.execution_id);
        }
    }
}
