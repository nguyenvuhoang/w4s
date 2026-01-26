using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ACT.API.Application.Models;

public class AccountChartCRUDReponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// AccountNumber
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// BranchCode
    /// </summary>
    public string BranchCode { get; set; }

    /// <summary>
    /// ParentAccountId
    /// </summary>
    public string ParentAccountId { get; set; }

    /// <summary>
    /// AccountLevel
    /// </summary>
    public int AccountLevel { get; set; }

    /// <summary>
    /// IsAccountLeave
    /// </summary>
    public bool IsAccountLeave { get; set; }

    /// <summary>
    /// BalanceSide
    /// </summary>
    public string BalanceSide { get; set; }

    /// <summary>
    /// ReverseBalance
    /// </summary>
    public string ReverseBalance { get; set; }

    /// <summary>
    /// PostingSide
    /// </summary>
    public string PostingSide { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// ShortAccountName
    /// </summary>
    public string ShortAccountName { get; set; }

    /// <summary>
    /// MultiValueName
    /// </summary>
    public MultiValueName MultiValueNameLang { get; set; }

    /// <summary>
    /// AccountClassification
    /// </summary>
    public string AccountClassification { get; set; }

    /// <summary>
    /// AccountCategories
    /// </summary>
    public string AccountCategories { get; set; }

    /// <summary>
    /// AccountGroup
    /// </summary>
    public string AccountGroup { get; set; }

    /// <summary>
    /// DirectPosting
    /// </summary>
    public string DirectPosting { get; set; }

    /// <summary>
    /// IsVisible
    /// </summary>
    public string IsVisible { get; set; }

    /// <summary>
    /// IsMultiCurrency
    /// </summary>
    public string IsMultiCurrency { get; set; }

    /// <summary>
    /// JobProcessOption
    /// </summary>
    public string JobProcessOption { get; set; }

    /// <summary>
    /// RefAccountNumber
    /// </summary>
    public string RefAccountNumber { get; set; }

    /// <summary>
    /// ReferencesNumber
    /// </summary>
    public string ReferencesNumber { get; set; }

    /// <summary>
    /// Is Cash Account
    /// </summary>
    public bool IsCashAccount { get; set; }

    /// <summary>
    /// Is Master Account
    /// </summary>
    public bool IsMasterAccount { get; set; }
}
