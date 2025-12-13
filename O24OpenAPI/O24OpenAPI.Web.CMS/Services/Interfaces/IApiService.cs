using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IApiService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="workflowId"></param>
    /// <param name="responseModel"></param>
    ///  <param name="executeId"></param>
    /// <param name="keyError"></param>
    /// <returns></returns>
    Task<List<ErrorInfoModel>> BuildError(
        string workflowId,
        WorkflowResponseModel responseModel,
        string executeId,
        string keyError = "SYSTEM_ERROR"
    );
}
