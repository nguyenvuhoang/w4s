using O24OpenAPI.CMS.API.Application.Models.Response;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

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
        string executeId,
        string keyError = "SYSTEM_ERROR"
    );
}
