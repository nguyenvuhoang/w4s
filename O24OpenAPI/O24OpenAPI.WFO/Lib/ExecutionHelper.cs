using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Logging;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;
using ILogger = O24OpenAPI.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.WFO.Lib;

public class ExecutionHelper
{
    public static async Task LogExecution(WorkflowExecution wfExecution)
    {
        try
        {
            Domain.WorkflowInfo wfi = wfExecution.execution.ToWorkflowInfo();
            await EngineContext.Current.Resolve<IWorkflowInfoService>().AddAsync(wfi);
            await DefaultLogger.CallLogGeneric(wfi, "WFO", "WORKFLOW_LOG");
            IWorkflowStepInfoService stepInfoService = EngineContext.Current.Resolve<IWorkflowStepInfoService>();
            foreach (WorkflowStepInfoModel item in wfExecution.execution_steps)
            {
                if (item.should_await)
                {
                    Domain.WorkflowStepInfo entity = item.ToWorkflowStepInfo();
                    await stepInfoService.AddAsync(entity);
                    await DefaultLogger.CallLogGeneric(entity, "WFO", "WORKFLOW_STEP_LOG");
                }
                else
                {
                    Domain.WorkflowStepInfo stepInfo = await stepInfoService.GetByExecutionStep(
                        wfExecution.execution.execution_id,
                        item.step_execution_id
                    );
                    if (stepInfo == null)
                    {
                        Domain.WorkflowStepInfo entity = item.ToWorkflowStepInfo();
                        await stepInfoService.AddAsync(entity);
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
