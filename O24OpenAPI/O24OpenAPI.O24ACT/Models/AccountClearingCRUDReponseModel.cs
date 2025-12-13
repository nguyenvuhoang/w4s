using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountClearingCRUDReponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// BranchCode
    /// </summary>
    public string BranchCode { get; set; }
    /// <summary>
    /// BranchName
    /// </summary>
    public string BranchName { get; set; }
    /// <summary>
    /// CurrencyId
    /// </summary>
    public string CurrencyId { get; set; }
    /// <summary>
    /// ClearingBranchCode
    /// </summary>
    public string ClearingBranchCode { get; set; }
    /// <summary>
    /// ClearingBranchName
    /// </summary>
    public string ClearingBranchName { get; set; }
    /// <summary>
    /// ClearingType
    /// </summary>
    public string ClearingType { get; set; }
    /// <summary>
    /// AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// Bank Account Number
    /// </summary>
    public string BankAccountNumber { get; set; }
}
