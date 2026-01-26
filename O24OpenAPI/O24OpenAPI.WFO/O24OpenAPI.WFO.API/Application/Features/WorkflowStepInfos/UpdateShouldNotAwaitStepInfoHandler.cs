using LinKit.Core.Abstractions;
using LinKit.Json.Runtime;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using O24OpenAPI.WFO.Infrastructure.Abstractions;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowStepInfos;

[RegisterService(Lifetime.Scoped)]
public class UpdateShouldNotAwaitStepInfoHandler(
    IWorkflowStepInfoRepository workflowStepInfoRepository
) : IUpdateShouldNotAwaitStepInfoHandler
{
    public async Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme)
    {
        try
        {
            WFScheme.REQUEST.REQUESTHEADER header = wFScheme.request.request_header;
            WFScheme.RESPONSE response = wFScheme.response;
            bool isSuccess = response.IsSuccess();

            WorkflowStepInfo stepInfo = await workflowStepInfoRepository.GetByExecutionStep(
                header.execution_id,
                header.step_execution_id
            );

            string content = isSuccess
                ? response.data.ToJson()
                : new Dictionary<string, object> { { "error", response.error_message } }.ToJson();

            int status = isSuccess
                ? (int)WorkflowExecutionStatus.Completed
                : (int)WorkflowExecutionStatus.Error;

            if (stepInfo != null)
            {
                stepInfo.p1_status = status;
                stepInfo.p1_error = response.error_code;
                stepInfo.p1_content = content;

                await workflowStepInfoRepository.Update(stepInfo);
            }
            else
            {
                stepInfo = new WorkflowStepInfo
                {
                    step_execution_id = header.step_execution_id,
                    execution_id = header.execution_id,
                    step_order = header.step_order,
                    step_code = header.step_code,
                    p1_request = wFScheme.request.ToJson(),
                    p1_start = header.utc_send_time,
                    p1_status = status,
                    p1_error = response.error_code,
                    p1_content = content,
                };

                await workflowStepInfoRepository.InsertAsync(stepInfo);
            }
        }
        catch (Exception ex)
        {
            ILogger logService = EngineContext.Current.Resolve<ILogger>();
            logService?.LogError(ex.Message, ex, null, wFScheme.request.request_header.execution_id);
        }
    }
}
