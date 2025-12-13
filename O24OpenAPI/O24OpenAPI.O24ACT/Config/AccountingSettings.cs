using Newtonsoft.Json;
using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.O24ACT.Configuration;

/// <summary>
/// Settings for Accounting
/// </summary>
public class AccountingSettings : ISettings
{
    /// <summary>
    /// Developer mode
    /// </summary>
    [JsonProperty("developer_mode")]
    public bool DeveloperMode { get; set; }

    /// <summary>
    /// Secret key
    /// </summary>
    [JsonProperty("secret_key")]
    public string SecretKey { get; set; }

    /// <summary>
    /// Token life time in days
    /// </summary>
    [JsonProperty("token_lifetime_days")]
    public int TokenLifetimeDays { get; set; }

    /// <summary>
    /// SystemAccountLevel
    /// </summary>
    [JsonProperty("account_leave_level")]
    public int AccountLeaveLevel { get; set; }

    /// <summary>
    /// SystemAccountNumberFormat
    /// </summary>
    [JsonProperty("system_account_number_format")]
    public string SystemAccountNumberFormat { get; set; }

    /// <summary>
    /// Length of Branch
    /// </summary>
    [JsonProperty("length_branch")]
    public int LengthBranch { get; set; } = 3;

    /// <summary>
    /// Length of Currency
    /// </summary>
    [JsonProperty("length_currency")]
    public int LengthCurrency { get; set; } = 3;

    /// <summary>
    /// Length of AccountNumber
    /// </summary>
    [JsonProperty("length_account_number")]
    public int LengthAccountNumber { get; set; } = 18;

    /// <summary>
    /// Length of Deposit AccountNumber
    /// </summary>
    [JsonProperty("length_deposit_account_number")]
    public int LengthDepositAccountNumber { get; set; } = 12;

    /// <summary>
    /// All Account Level
    /// </summary>
    [JsonProperty("all_account_level")]
    public int AllAccountLevel { get; set; }

    /// <summary>
    /// All Currency Code
    /// </summary>
    [JsonProperty("all_currency_code")]
    public string AllCurrencyCode { get; set; }

    /// <summary>
    /// Is enable balance change
    /// </summary>
    [JsonProperty("enable_balance")]
    public bool EnableBalance { get; set; }

    /// <summary>
    /// CurrentBaseCurrency
    /// </summary>
    [JsonProperty("current_base_currency")]
    public string CurrentBaseCurrency { get; set; }

    /// <summary>
    /// FX_IBT
    /// </summary>
    [JsonProperty("fx_ibt")]
    public bool FX_IBT { get; set; }

    /// <summary>
    /// FormatDate
    /// </summary>
    [JsonProperty("format_date")]
    public string FormatDate { get; set; }

    /// <summary>
    /// AutoOpenAccount
    /// </summary>
    [JsonProperty("auto_open_account")]
    public bool AutoOpenAccount { get; set; }

    /// <summary>
    /// DurationBackDateNumber
    /// </summary>
    [JsonProperty("duration_back_date_number")]
    public int DurationBackDateNumber { get; set; }

    /// <summary>
    /// DurationBackDateUnit
    /// </summary>
    [JsonProperty("duration_back_date_unit")]
    public string DurationBackDateUnit { get; set; } = "D";

    /// <summary>
    /// SystemLanguage
    /// </summary>
    [JsonProperty("system_language")]
    public string SystemLanguage { get; set; }

    /// <summary>
    /// BackupDirectory
    /// </summary>
    [JsonProperty("backup_directory")]
    public string BackupDirectory { get; set; }

    /// <summary>
    /// AllowPostingWithLevel
    /// </summary>
    [JsonProperty("allow_posting_with_level")]
    public string AllowPostingWithLevel { get; set; } = "[9]";

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ibt_via_ho")]
    public bool IbtViaHo { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("host_branch_code")]
    public string HostBranchCode { get; set; } = "000";
}
