using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Models;

public class OpenFXAccountModel : BaseTransactionModel
{
    /// <summary>
    /// Constructor
    /// </summary>
    public OpenFXAccountModel() { }

    /// <summary>
    /// dorc
    /// </summary>
    public string DebitOrCredit { get; set; }

    /// <summary>
    /// BranchAccountNumber
    /// </summary>
    public string BranchGLBankAccountNumber { get; set; }

    /// <summary>
    /// CurrencyCodeGLBankAccountNumber
    /// </summary>
    public string CurrencyCodeGLBankAccountNumber { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// bamt
    /// </summary>
    public string AccountNumber { get; set; }
}
