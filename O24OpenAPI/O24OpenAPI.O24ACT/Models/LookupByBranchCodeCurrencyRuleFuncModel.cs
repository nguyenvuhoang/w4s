using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class LookupByBranchCodeCurrencyRuleFuncModel : BaseTransactionModel
{
    /// <summary>
    /// Contructor
    /// </summary>
    public LookupByBranchCodeCurrencyRuleFuncModel() { }

    /// <summary>
    /// AccountGroup
    /// </summary>
    public string NotInAccountGroup { get; set; }

    /// <summary>
    /// BankAccountNumber
    /// </summary>
    public string AccountBranchCode { get; set; }

    /// <summary>
    /// CurrencyCode
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
