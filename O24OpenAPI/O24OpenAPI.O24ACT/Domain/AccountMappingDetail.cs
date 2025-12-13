using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

//d_acmaprl
/// <summary>
/// AccountMappingDetail
/// </summary>
public partial class AccountMappingDetail : BaseEntity
{
    /// <summary>
    /// AccountMappingDetail
    /// </summary>
    public AccountMappingDetail() { }
    /// <summary>
    /// MappingId
    /// </summary>
    [JsonProperty("mapping_id")]
    public string MappingId { get; set; }
    /// <summary>
    /// SystemAccountNumber
    /// </summary>
    [JsonProperty("system_account_number")]
    public string SystemAccountNumber { get; set; }
    /// <summary>
    /// BankAccountNumber
    /// </summary>
    [JsonProperty("bank_account_number")]
    public string BankAccountNumber { get; set; }

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
