using LinKit.Core.Abstractions;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Diagnostics;

namespace O24OpenAPI.WFO.API.Application.Features.Worklfows
{
    public interface ISearchWorkflowHandler
    {
        Task<WorkflowResponse> SimpleSearch(SimpleSearchModel simpleSearchModel);
    }

    [RegisterService(Lifetime.Scoped)]
    public class SearchWorkflowHandler(
        IWorkflowDefRepository workflowDefRepository,
        WorkContext workContext
    ) : ISearchWorkflowHandler
    {
        public async Task<WorkflowResponse> SimpleSearch(SimpleSearchModel simpleSearchModel)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            WorkflowResponse response = new()
            {
                execution_id = workContext.ExecutionId,
                transaction_date = DateTime.UtcNow,
            };
            try
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
                stopwatch.Stop();

                response.data = pageListModel.ToDictionary();
                response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
                response.status = WorkflowExecutionStatus.Completed.ToString();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                response.data = [];
                response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
                response.status = WorkflowExecutionStatus.Error.ToString();
                response.error_message = ex.Message;
                response.error_code = ex.HResult.ToString("X8");
            }
            return response;
        }
    }
}
