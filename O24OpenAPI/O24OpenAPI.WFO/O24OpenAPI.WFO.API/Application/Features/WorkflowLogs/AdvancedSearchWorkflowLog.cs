using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;

public class AdvancedSearchWorkflowLogQuery
    : BaseSearch,
        IQuery<PagedListModel<AdvancedSearchWorkflowLogResponse>>
{
    [JsonPropertyName("Workflowid")]
    public string WorkflowId { get; set; }

    [JsonPropertyName("execution_id")]
    public string ExecutionId { get; set; }

    [JsonPropertyName("from_date")]
    public DateTime? FromDate { get; set; }

    [JsonPropertyName("to_date")]
    public DateTime? ToDate { get; set; }
}

public class AdvancedSearchWorkflowLogResponse : BaseO24OpenAPIModel
{
    public string execution_id { get; set; }

    public string workflow_id { get; set; }

    public int status { get; set; }

    public string error { get; set; }

    public long created_on { get; set; }

    public long finish_on { get; set; }

    public string is_success { get; set; }

    public string workflow_type { get; set; }

    public string reversed_execution_id { get; set; }

    public string reversed_by_execution_id { get; set; }
}

[CqrsHandler]
public class AdvancedSearchWorkflowLogHandler(IWorkflowInfoRepository workflowInfoRepository)
    : IQueryHandler<
        AdvancedSearchWorkflowLogQuery,
        PagedListModel<AdvancedSearchWorkflowLogResponse>
    >
{
    public async Task<PagedListModel<AdvancedSearchWorkflowLogResponse>> HandleAsync(
        AdvancedSearchWorkflowLogQuery request,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<WorkflowInfo> q = workflowInfoRepository.Table;
        if (!string.IsNullOrWhiteSpace(request.WorkflowId))
        {
            q.Where(s => s.workflow_id == request.WorkflowId);
        }
        if (!string.IsNullOrWhiteSpace(request.ExecutionId))
        {
            q.Where(s => s.execution_id == request.ExecutionId);
        }
        if (request.FromDate.HasValue)
        {
            q.Where(s => s.created_on >= request.FromDate.ToUnixTimeMilliseconds());
        }
        if (request.ToDate.HasValue)
        {
            q.Where(s => s.created_on <= request.ToDate.ToUnixTimeMilliseconds());
        }
        List<WorkflowInfo> list = await q.ToListAsync(request.PageIndex, request.PageSize);
        List<AdvancedSearchWorkflowLogResponse> listWorkflowInfoModel =
            list.ToAdvancedSearchWorkflowLogResponseList();
        return listWorkflowInfoModel.ToPagedListModel(request.PageIndex, request.PageSize);
    }
}
