using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Neptune;

namespace O24OpenAPI.Web.CMS.Services.NeptuneService;

public interface INeptuneCBSService
{
    Task<ResponseModel> CallApiAsync(CallApiModel callApiModel, string refid);
    Task<ResponseModel> ExecuteWorkflow(ExecuteWorkflowNeptuneModel model);
}
