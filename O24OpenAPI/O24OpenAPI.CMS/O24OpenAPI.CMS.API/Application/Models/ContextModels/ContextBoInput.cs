using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.CMS.API.Application.Utils;

namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

/// <summary>
///
/// </summary>
public class ContextBoInputModel
{
    // public ContextBoInputModel() { }
    /// <summary>
    ///
    /// </summary>
    // private string txcode = "";
    private JObject boInput = [];
    private readonly FoResponseModel<object> foInput = new();
    private JObject actionInput = [];
    private List<ErrorInfoModel> actionError = [];
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

    public void SetBoInput(JObject boInput_)
    {
        boInput = boInput_;
    }

    public void SetActionInput(JObject actionInput_)
    {
        actionInput = actionInput_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public FoResponseModel<object> GetFoInput()
    {
        return foInput;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public JObject GetActionInput()
    {
        return actionInput;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public JObject GetBoInput()
    {
        return boInput;
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    public void AddErrorRunRule(ErrorStatusModel error)
    {
        JObject obError = new() { ["count"] = error.GetCode() };
        AddPackFo("errorJWebUI", obError);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="arr"></param>
    public void AddActionErrorAll(List<ErrorInfoModel> arr)
    {
        actionError.AddRange(arr);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="arr"></param>
    public void setActionErrorAll(List<ErrorInfoModel> arr)
    {
        actionError = arr;
    }
}
