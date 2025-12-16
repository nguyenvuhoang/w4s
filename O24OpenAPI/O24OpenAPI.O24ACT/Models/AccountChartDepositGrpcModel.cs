using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class AccountChartDepositGrpcModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// BankAccountNumber
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// AccountName
    /// </summary>
    public string AccountName { get; set; }
    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }
    /// <summary>
    /// IsAccountLeave
    /// </summary>
    public bool IsAccountLeave { get; set; }
    /// <summary>
    /// DirectPosting
    /// </summary>
    public string DirectPosting { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string DepositStatus { get; set; } = string.Empty;
    /// <summary>
    ///
    /// </summary>
    public string DepositType { get; set; } = string.Empty;
    /// <summary>
    /// BranchCode
    /// /// </summary>
    public string BranchCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string RecBy { get; set; } = "ACT";
}
