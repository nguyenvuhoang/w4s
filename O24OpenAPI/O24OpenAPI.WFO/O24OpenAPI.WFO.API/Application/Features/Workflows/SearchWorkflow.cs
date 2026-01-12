using LinKit.Core.Abstractions;
using O24OpenAPI.Core;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Features.Workflows;

public interface ISearchWorkflowHandler
{
    Task<PagedListModel<WorkflowDef, WorkflowDefSearchResponse>> SimpleSearch(
        SimpleSearchModel simpleSearchModel
    );
}

[RegisterService(Lifetime.Scoped)]
public class SearchWorkflowHandler(IWorkflowDefRepository workflowDefRepository)
    : ISearchWorkflowHandler
{
    public async Task<PagedListModel<WorkflowDef, WorkflowDefSearchResponse>> SimpleSearch(
        SimpleSearchModel simpleSearchModel
    )
    {
        IQueryable<WorkflowDef> query =
            from d in workflowDefRepository.Table
            where
                string.IsNullOrEmpty(simpleSearchModel.SearchText)
                || d.WorkflowId.Contains(simpleSearchModel.SearchText)
            select d;
        IPagedList<WorkflowDef> result = await query.ToPagedList(
            simpleSearchModel.PageIndex,
            simpleSearchModel.PageSize
        );
        PagedListModel<WorkflowDef, WorkflowDefSearchResponse> pageListModel =
            result.ToPagedListModel<WorkflowDef, WorkflowDefSearchResponse>();
        return pageListModel;
    }
}
