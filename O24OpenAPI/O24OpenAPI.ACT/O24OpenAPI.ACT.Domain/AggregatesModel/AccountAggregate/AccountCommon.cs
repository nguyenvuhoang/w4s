using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

/// <summary>
/// AccountCommon
/// </summary>
public partial class AccountCommon : BaseEntity
{
    /// <summary>
    /// AccountCommon
    /// </summary>
    public AccountCommon() { }

    /// <summary>
    /// AccountNumber
    /// </summary>
    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    [JsonProperty("account_name")]
    public string AccountName { get; set; }

    /// <summary>
    /// RefAccountNumber
    /// </summary>
    [JsonProperty("ref_account_number")]
    public string RefAccountNumber { get; set; }

    /// <summary>
    /// RefAccountNumber2
    /// </summary>
    [JsonProperty("ref_account_number2")]
    public string RefAccountNumber2 { get; set; }

    /// <summary>
    /// create
    /// </summary>
    [JsonProperty("created_on_utc")]
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    [JsonProperty("updated_on_utc")]
    public DateTime? UpdatedOnUtc { get; set; }
}
