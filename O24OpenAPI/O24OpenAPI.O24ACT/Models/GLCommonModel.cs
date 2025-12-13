using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class GLCommonModel : BaseTransactionModel
{
    public GLCommonModel()
    {

    }
    public string AccountCommon { get; set; }
    public string CurrencyCode { get; set; }
    public string DorC { get; set; }
    public decimal Amount { get; set; }
    public string BranchCode { get; set; }
    public int AccountingGroup { get; set; } = 1;
}
