using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class ApiService : IApiService
{
    /// <summary>
    ///
    /// </summary>
    protected ErrorInfoModel AddActionError(
        string type,
        string typeError,
        string info,
        string code,
        string executeId,
        string key
    )
    {
        return new ErrorInfoModel()
        {
            type = type,
            typeError = typeError,
            key = key,
            info = info,
            code = code,
            executeId = executeId,
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<ErrorInfoModel>> BuildError(
        string workflowId,
        WorkflowResponseModel responseModel,
        string executeId,
        string keyError = "SYSTEM_ERROR"
    )
    {
        List<ErrorInfoModel> listError = new();
        try
        {
            if (responseModel.status != 0)
            {
                listError.Add(
                    AddActionError(
                        ErrorType.errorForm,
                        ErrorMainForm.warning,
                        responseModel.error_message,
                        workflowId,
                        executeId,
                        responseModel.error_code
                    )
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }

        await Task.CompletedTask;

        return listError;
    }
}
