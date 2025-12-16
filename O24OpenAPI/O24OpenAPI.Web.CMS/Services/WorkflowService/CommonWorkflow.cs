using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class CommonWorkflow(ICommonService commonService) : BaseQueueService
{
    private readonly ICommonService _commonService = commonService;

    public async Task<WorkflowScheme> GetPagedList(WorkflowScheme workflowScheme)
    {
        var model = await workflowScheme.ToModel<SearchPagedListModel>();
        return await Invoke<BaseTransactionModel>(
            workflowScheme,
            async () =>
            {
                var result = await _commonService.GetPagedList(model);
                var response = new
                {
                    page_index = result.PageIndex,
                    page_size = result.PageSize,
                    total_count = result.TotalCount,
                    total_pages = result.TotalPages,
                    has_previous_page = result.HasPreviousPage,
                    has_next_page = result.HasNextPage,
                    items = result,
                };
                return response;
            }
        );
    }
}
