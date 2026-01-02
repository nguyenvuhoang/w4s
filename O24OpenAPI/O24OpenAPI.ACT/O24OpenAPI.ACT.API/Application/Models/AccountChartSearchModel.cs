using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Models;

public partial class AccountChartSearchModel : BaseTransactionModel
{
    /// <summary>
    /// AccountChartUpdateModel
    /// </summary>
    public AccountChartSearchModel()
    {
        this.PageIndex = 0;
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    /// BankAccountNumber
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int? AccountLevelFrom { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int? AccountLevelTo { get; set; }

    /// <summary>
    /// BalanceSide
    /// </summary>
    public string BalanceSide { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// AccountClassification
    /// </summary>
    public string AccountClassification { get; set; }

    /// <summary>
    /// AccountGroup
    /// </summary>
    public string AccountGroup { get; set; }

    /// <summary>
    /// AccountGroup
    /// </summary>
    public string NotInAccountGroup { get; set; }

    /// <summary>
    /// NotInDirectPosting
    /// </summary>
    public string NotInDirectPosting { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DirectPosting { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool? IsAccountLeave { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Refresh Cache
    /// </summary>
    public bool? RefreshCache { get; set; } = false;
}
