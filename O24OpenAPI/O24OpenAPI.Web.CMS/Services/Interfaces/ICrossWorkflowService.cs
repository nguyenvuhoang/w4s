using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services;

public partial interface ICrossWorkflowService
{
    public Task<WorkflowScheme> CallApiAsync(CallApiModel callApiModel);
}
