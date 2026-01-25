using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

/// <summary>
///
/// </summary>
public class ContextBoInputModel
{
    private readonly JObject boInput = [];
    private readonly FoResponseModel<object> foInput = new();
    private readonly JObject actionInput = [];
    private readonly List<ErrorInfoModel> actionError = [];
    private readonly JObject actionTrace = [];

    /// <summary>
    ///
    /// </summary>
    public bool isDebug = false;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<ErrorInfoModel> GetActionErrors()
    {
        return actionError;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name_"></param>
    /// <param name="detail_"></param>
    public void AddPackFo<InputValueType>(string name_, InputValueType detail_)
    {
        foInput.input[name_] = detail_;
        actionInput[name_] = detail_.ToJToken();
    }
}
