using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;

public class GetWorkflowLogByExecutionIdQuery
    : BaseSearch,
        IQuery<GetWorkflowLogByExecutionIdResponse>
{
    [JsonPropertyName("execution_id")]
    public string ExecutionId { get; set; }
}

public class GetWorkflowLogByExecutionIdResponse
{
    [JsonPropertyName("workflow_info")]
    public WorkflowInfoModel WorkflowInfo { get; set; }

    [JsonPropertyName("workflow_step_info_list")]
    public List<WorkflowStepInfoModel> WorkflowStepInfoModelList { get; set; }
}

[CqrsHandler]
public class GetWorkflowLogByExecutionIdHandler(
    IWorkflowStepInfoRepository workflowStepInfoRepository,
    IWorkflowInfoRepository workflowInfoRepository
) : IQueryHandler<GetWorkflowLogByExecutionIdQuery, GetWorkflowLogByExecutionIdResponse>
{
    public async Task<GetWorkflowLogByExecutionIdResponse> HandleAsync(
        GetWorkflowLogByExecutionIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        WorkflowInfo workflowInfo = await workflowInfoRepository.GetByExecutionIdAsync(
            request.ExecutionId
        );
        List<WorkflowStepInfo> list = await workflowStepInfoRepository.GetByExecutionId(
            request.ExecutionId
        );
        List<WorkflowStepInfoModel> listModel = list.ToWorkflowStepInfoModelList();
        return new GetWorkflowLogByExecutionIdResponse
        {
            WorkflowInfo = workflowInfo.ToWorkflowInfoModel(),
            WorkflowStepInfoModelList = listModel,
        };
    }
}
