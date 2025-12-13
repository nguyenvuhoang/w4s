using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

/// <summary>
/// Account Balance
/// </summary>
public partial class AccountBalance : BaseEntity
{
    /// <summary>
    /// AccountNumber
    /// </summary>
    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("branch_code")]
    public string BranchCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Balance
    /// </summary>
    [JsonProperty("balance")]
    public decimal Balance { get; set; }

    /// <summary>
    /// DailyDebit
    /// </summary>
    [JsonProperty("daily_debit")]
    public decimal DailyDebit { get; set; }

    /// <summary>
    /// DailyCredit
    /// </summary>
    [JsonProperty("daily_credit")]
    public decimal DailyCredit { get; set; }

    /// <summary>
    /// WeeklyDebit
    /// </summary>
    [JsonProperty("weekly_debit")]
    public decimal WeeklyDebit { get; set; }

    /// <summary>
    /// WeeklyCredit
    /// </summary>
    [JsonProperty("weekly_credit")]
    public decimal WeeklyCredit { get; set; }

    /// <summary>
    /// MonthlyDebit
    /// </summary>
    [JsonProperty("monthly_debit")]
    public decimal MonthlyDebit { get; set; }

    /// <summary>
    /// MonthlyCredit
    /// </summary>
    [JsonProperty("monthly_credit")]
    public decimal MonthlyCredit { get; set; }

    /// <summary>
    /// QuaterlyDebit
    /// </summary>
    [JsonProperty("quarterly_debit")]
    public decimal QuarterlyDebit { get; set; }

    /// <summary>
    /// QuaterlyCredit
    /// </summary>
    [JsonProperty("quarterly_credit")]
    public decimal QuarterlyCredit { get; set; }

    /// <summary>
    /// HalfYearlyDebit
    /// </summary>
    [JsonProperty("half_yearly_debit")]
    public decimal HalfYearlyDebit { get; set; }

    /// <summary>
    /// HalfYearlyCredit
    /// </summary>
    [JsonProperty("half_yearly_credit")]
    public decimal HalfYearlyCredit { get; set; }

    /// <summary>
    /// YearlyDebit
    /// </summary>
    [JsonProperty("yearly_debit")]
    public decimal YearlyDebit { get; set; }

    /// <summary>
    /// YearlyCredit
    /// </summary>
    [JsonProperty("yearly_credit")]
    public decimal YearlyCredit { get; set; }

    /// <summary>
    /// WeekAverageBalance
    /// </summary>
    [JsonProperty("week_average_balance")]
    public decimal WeekAverageBalance { get; set; }
    /// <summary>
    /// MonthAverageBalance
    /// </summary>
    [JsonProperty("month_average_balance")]
    public decimal MonthAverageBalance { get; set; }

    /// <summary>
    /// QuarterAverageBalance
    /// </summary>
    [JsonProperty("quarter_average_balance")]
    public decimal QuarterAverageBalance { get; set; }

    /// <summary>
    /// HalfYearAverageBalance
    /// </summary>
    [JsonProperty("half_year_average_balance")]
    public decimal HalfYearAverageBalance { get; set; }

    /// <summary>
    /// YearAverageBalance
    /// </summary>
    [JsonProperty("year_average_balance")]
    public decimal YearAverageBalance { get; set; }

    /// <summary>
    /// create date
    /// </summary>
    [JsonProperty("created_on_utc")]
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    [JsonProperty("updated_on_utc")]
    public DateTime? UpdatedOnUtc { get; set; }
}
