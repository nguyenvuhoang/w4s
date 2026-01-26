using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Logging;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using ILogger = O24OpenAPI.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.WFO.API.Application.Utils;

public class ExecutionHelper
{
    public static async Task LogExecution(WorkflowExecution wfExecution)
    {
        try
        {
            WorkflowInfo wfi = wfExecution.execution.ToWorkflowInfo();
            await EngineContext.Current.Resolve<IWorkflowInfoRepository>().InsertAsync(wfi);
            await DefaultLogger.CallLogGeneric(wfi, "WFO", "WORKFLOW_LOG");
            IWorkflowStepInfoRepository workflowStepInfoRepository =
                EngineContext.Current.Resolve<IWorkflowStepInfoRepository>();
            foreach (WorkflowStepInfoModel item in wfExecution.execution_steps)
            {
                if (item.should_await)
                {
                    WorkflowStepInfo entity = item.ToWorkflowStepInfo();
                    await workflowStepInfoRepository.InsertAsync(entity);
                    await DefaultLogger.CallLogGeneric(entity, "WFO", "WORKFLOW_STEP_LOG");
                }
                else
                {
                    WorkflowStepInfo stepInfo = await workflowStepInfoRepository.GetByExecutionStep(
                        wfExecution.execution.execution_id,
                        item.step_execution_id
                    );
                    if (stepInfo == null)
                    {
                        WorkflowStepInfo entity = item.ToWorkflowStepInfo();
                        await workflowStepInfoRepository.InsertAsync(entity);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ILogger logService = EngineContext.Current.Resolve<ILogger>();
            WorkflowExecution _wfExecution = EngineContext.Current.Resolve<WorkflowExecution>();
            await logService.Error(ex.Message, ex, null, _wfExecution.execution.execution_id);
        }
    }
}
