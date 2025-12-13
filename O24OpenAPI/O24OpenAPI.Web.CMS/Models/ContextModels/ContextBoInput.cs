using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models;

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
    private JObject boInput = new JObject();
    private FoResponseModel<object> foInput = new FoResponseModel<object>();
    private JObject actionInput = new JObject();
    private List<ErrorInfoModel> actionError = new List<ErrorInfoModel>();
    private JObject actionTrace = new JObject();

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
        JObject obError = new JObject();
        obError.Add(new JProperty("count", error.GetCode()));
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
