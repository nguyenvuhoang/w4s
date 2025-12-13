using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Services.Logging;

public interface IWorkflowStepLogService
{
    Task LogCallApiNeptune(CallApiModel model, string refid);
    Task UpdateLogNeptune(
        string refid,
        string stepCode,
        string status,
        string responseData
    );
}
