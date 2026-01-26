using LinKit.Core.Abstractions;
using LinKit.Json.Runtime;
using LinqToDB;
using Microsoft.Extensions.Logging;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;
using static O24OpenAPI.Client.Workflow.WorkflowExecution;

namespace O24OpenAPI.Logger.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class WorkflowStepLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WorkflowStepLog>(dataProvider, staticCacheManager), IWorkflowStepLogRepository
{
    public async Task<List<WorkflowStepLog>> GetByExecutionIdAsync(string executionId)
    {
        List<WorkflowStepLog> logs = await Table
            .Where(x => x.execution_id == executionId)
            .ToListAsync();
        return logs;
    }

    public async Task<WorkflowStepLog?> GetByIdAsync(string stepExecutionId)
    {
        IQueryable<WorkflowStepLog> query = Table;
        WorkflowStepLog? log = await query.FirstOrDefaultAsync(x =>
            x.step_execution_id == stepExecutionId
        );
        return log;
    }

    public async Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme)
    {
        try
        {
            WFScheme.REQUEST.REQUESTHEADER header = wFScheme.request.request_header;
            WFScheme.RESPONSE response = wFScheme.response;
            bool isSuccess = response.IsSuccess();

            WorkflowStepLog? stepInfo = await GetByIdAsync(header.step_execution_id);

            string content = isSuccess
                ? response.data.ToJson(o => o.WriteIndented = true)
                : new Dictionary<string, object> { { "error", response.error_message } }.ToJson(o =>
                    o.WriteIndented = true
                );

            int status = isSuccess ? (int)Enum_STATUS.Completed : (int)Enum_STATUS.Exception;

            if (stepInfo != null)
            {
                stepInfo.p1_status = status;
                stepInfo.p1_error = response.error_code;
                stepInfo.p1_content = content;

                await Update(stepInfo);
            }
            else
            {
                stepInfo = new WorkflowStepLog
                {
                    step_execution_id = header.step_execution_id,
                    execution_id = header.execution_id,
                    step_order = header.step_order,
                    step_code = header.step_code,
                    p1_request = wFScheme.request.ToJson(o => o.WriteIndented = true),
                    p1_start = header.utc_send_time,
                    p1_status = status,
                    p1_error = response.error_code,
                    p1_content = content,
                };

                await InsertAsync(stepInfo);
            }
        }
        catch (Exception ex)
        {
            ILogger? logService = EngineContext.Current.Resolve<ILogger>();
            logService?.LogError(
                ex.Message,
                ex,
                null,
                wFScheme.request.request_header.step_execution_id
            );
        }
    }
}
