using Newtonsoft.Json;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

[Auditable]
public partial class AccountCommon : BaseEntity
{
    /// <summary>
    /// AccountNumber
    /// </summary>
    [JsonProperty("account_number")]
    public string? AccountNumber { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    [JsonProperty("account_name")]
    public string? AccountName { get; set; }

    /// <summary>
    /// RefAccountNumber
    /// </summary>
    [JsonProperty("ref_account_number")]
    public string? RefAccountNumber { get; set; }

    /// <summary>
    /// RefAccountNumber2
    /// </summary>
    [JsonProperty("ref_account_number2")]
    public string? RefAccountNumber2 { get; set; }
}
