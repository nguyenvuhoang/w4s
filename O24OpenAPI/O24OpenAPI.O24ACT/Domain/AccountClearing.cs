using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

////S_ACCLR
/// <summary>
/// AccountClearing
/// </summary>
public partial class AccountClearing : BaseEntity
{
    /// <summary>
    /// AccountClearing
    /// </summary>
    public AccountClearing() { }

    /// <summary>
    /// BranchCodeOriginal
    /// </summary>
    [JsonProperty("branch_code")]
    public string BranchCode { get; set; }

    /// <summary>
    /// CurrencyId
    /// </summary>
    [JsonProperty("currency_code")]
    public string CurrencyId { get; set; }

    /// <summary>
    /// ClearingId
    /// </summary>
    [JsonProperty("clearing_branch_code")]
    public string ClearingBranchCode { get; set; }

    /// <summary>
    /// ClearingType
    /// </summary>
    [JsonProperty("clearing_type")]
    public string ClearingType { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    [JsonProperty("account_name")]
    public string AccountName { get; set; }

    /// <summary>
    /// AccountNumber
    /// </summary>
    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    [JsonProperty("created_on_utc")]
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    [JsonProperty("updated_on_utc")]
    public DateTime? UpdatedOnUtc { get; set; }
}
