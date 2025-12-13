using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public partial class LookupByCurrencyRuleFuncModel : BaseTransactionModel
{
    /// <summary>
    /// AccountGroup
    /// </summary>
    public string NotInAccountGroup { get; set; }

    /// <summary>
    /// BankAccountNumber
    /// </summary>
    public string CurrencyCode { get; set; }
    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; } = 0;
    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}
