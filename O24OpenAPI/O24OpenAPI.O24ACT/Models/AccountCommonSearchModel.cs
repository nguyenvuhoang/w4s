using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountCommonSearchModel : BaseTransactionModel
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AccountCommonSearchModel()
    {
        this.PageIndex = 0;
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    /// AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// RefAccountNumber
    /// </summary>
    public string RefAccountNumber { get; set; }

    /// <summary>
    /// RefAccountNumber2
    /// </summary>
    public string RefAccountNumber2 { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; }
}
