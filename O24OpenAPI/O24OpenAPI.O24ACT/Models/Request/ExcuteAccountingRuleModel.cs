using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models.Request;

public class ExcuteAccountingRuleModel : BaseTransactionModel
{
    public ExcuteAccountingRuleModel()
    {
        ListGL = [];
        ListGLFromResponse = [];
        GLCommon = new GLCommonModel();
        ListGLCommon = [];
    }

    public List<GLEntriesModel> ListGL { get; set; }
    public List<GLEntriesFromResponseModel> ListGLFromResponse { get; set; }
    public GLCommonModel GLCommon { get; set; }
    public List<GLCommonModel> ListGLCommon { get; set; }
    public dynamic Fields { get; set; }
    public string DigitalTransactionId { get; set; }
}
