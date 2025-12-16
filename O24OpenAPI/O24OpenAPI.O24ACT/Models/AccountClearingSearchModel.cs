using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountClearingSearchModel : BaseTransactionModel
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AccountClearingSearchModel()
    {
        this.PageIndex = 0;
        this.PageSize = int.MaxValue;
    }
    /// <summary>
    /// brname
    /// </summary>
    public string BranchName { get; set; }
    /// <summary>
    /// CurrencyId
    /// </summary>
    public string CurrencyId { get; set; }
    /// <summary>
    /// ClearingBranchCodect
    /// </summary>
    public string ClearingBranchName { get; set; }
    /// <summary>
    /// ClearingTypect
    /// </summary>
    public string ClearingType { get; set; }
    /// <summary>
    /// AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; }
}
