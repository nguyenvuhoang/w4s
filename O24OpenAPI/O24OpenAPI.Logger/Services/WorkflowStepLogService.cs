using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Models.Workflow;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;
using static O24OpenAPI.O24OpenAPIClient.Workflow.WorkflowExecution;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Logger.Services;

/// <summary>
/// The workflow step log service class
/// </summary>
/// <seealso cref="ILogService{WorkflowStepLog}"/>
public class WorkflowStepLogService(IRepository<WorkflowStepLog> repo)
    : ILogService<WorkflowStepLog>,
        IWorkflowStepLogService
{
    /// <summary>
    /// The repo
    /// </summary>
    private readonly IRepository<WorkflowStepLog> _repo = repo;

    /// <summary>
    /// Adds the log
    /// </summary>
    /// <param name="log">The log</param>
    public async Task AddAsync(WorkflowStepLog log)
    {
        await _repo.Insert(log);
    }

    /// <summary>
    /// Simples the search using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>The result</returns>
    public async Task<PagedModel> SimpleSearch(SearchModel model)
    {
        var query = _repo.Table;
        var pageList = await query.ToPagedList(model.PageIndex, model.PageSize);
        var result = pageList.ToPagedListModel<
            WorkflowStepLog,
            WorkflowStepLogSearchResponse
        >();
        return result;
    }

    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    public async Task<WorkflowStepLog> GetByExecutionIdAsync(string executionId)
    {
        var query = _repo.Table;
        var log = await query.FirstOrDefaultAsync(x => x.execution_id == executionId);
        return log;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stepExecutionId"></param>
    /// <returns></returns>
    public async Task<WorkflowStepLog> GetByIdAsync(string stepExecutionId)
    {
        var query = _repo.Table;
        var log = await query.FirstOrDefaultAsync(x => x.step_execution_id == stepExecutionId);
        return log;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    public async Task UpdateAsync(WorkflowStepLog log)
    {
        await _repo.Update(log);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme)
    {
        try
        {
            var header = wFScheme.request.request_header;
            var response = wFScheme.response;
            var isSuccess = response.IsSuccess();

            var stepInfo = await GetByIdAsync(header.step_execution_id);

            var content = isSuccess
                ? response.data.ToSerialize()
                : new Dictionary<string, object>
                {
                    { "error", response.error_message },
                }.ToSerialize();

            var status = isSuccess ? (int)Enum_STATUS.Completed : (int)Enum_STATUS.Exception;

            if (stepInfo != null)
            {
                stepInfo.p1_status = status;
                stepInfo.p1_error = response.error_code;
                stepInfo.p1_content = content;

                await UpdateAsync(stepInfo);
            }
            else
            {
                stepInfo = new WorkflowStepLog
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
            logService.LogError(
                ex.Message,
                ex,
                null,
                wFScheme.request.request_header.step_execution_id
            );
        }
    }
}
