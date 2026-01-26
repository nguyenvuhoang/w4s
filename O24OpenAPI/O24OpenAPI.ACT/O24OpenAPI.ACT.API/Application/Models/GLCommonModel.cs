using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Models;

public class GLCommonModel : BaseTransactionModel
{
    public GLCommonModel() { }

    public string AccountCommon { get; set; }
    public string CurrencyCode { get; set; }
    public string DorC { get; set; }
    public decimal Amount { get; set; }
    public string BranchCode { get; set; }
    public int AccountingGroup { get; set; } = 1;
}
