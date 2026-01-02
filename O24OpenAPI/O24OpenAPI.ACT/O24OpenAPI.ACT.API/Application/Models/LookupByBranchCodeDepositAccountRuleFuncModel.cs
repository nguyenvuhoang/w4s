using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Models;

public class LookupByBranchCodeDepositAccountRuleFuncModel : BaseTransactionModel
{
    /// <summary>
    /// Contructor
    /// </summary>
    public LookupByBranchCodeDepositAccountRuleFuncModel() { }

    /// <summary>
    /// AccountGroup
    /// </summary>
    public string NotInAccountGroup { get; set; }

    /// <summary>
    /// AccountBranchCode
    /// </summary>
    public string AccountBranchCode { get; set; }

    /// <summary>
    /// AccountBranchCode
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}
