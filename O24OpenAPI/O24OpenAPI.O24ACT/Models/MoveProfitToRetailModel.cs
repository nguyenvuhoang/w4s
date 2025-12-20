using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class MoveProfitToRetailModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Profit Account
    /// </summary>
    public string ProfitAccount { get; set; }

    /// <summary>
    /// Deprecate amount
    /// </summary>
    public string AccountType { get; set; }

    /// <summary>
    /// Debit account
    /// </summary>
    public string DebitAccount { get; set; }

    /// <summary>
    /// Balance
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Branch code
    /// </summary>
    public string BranchCode { get; set; }
}
