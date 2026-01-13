using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;

public class GetWorkflowStepLogByExecutionIdQuery
    : BaseSearch, IQuery<PagedListModel<WorkflowStepInfoModel>>
{
    [JsonPropertyName("execution_id")]
    public string ExecutionId { get; set; }
}

[CqrsHandler]
public class GetWorkflowStepLogByExecutionIdHandler(IWorkflowStepInfoRepository workflowStepInfoRepository)
    : IQueryHandler<GetWorkflowStepLogByExecutionIdQuery, PagedListModel<WorkflowStepInfoModel>>
{
    public async Task<PagedListModel<WorkflowStepInfoModel>> HandleAsync(
        GetWorkflowStepLogByExecutionIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        List<WorkflowStepInfo> list = await workflowStepInfoRepository.GetByExecutionId(request.ExecutionId);
        List<WorkflowStepInfoModel> listModel = list.ToWorkflowStepInfoModelList();
        return listModel.ToPagedListModel(request.PageIndex, request.PageSize);
    }
}
