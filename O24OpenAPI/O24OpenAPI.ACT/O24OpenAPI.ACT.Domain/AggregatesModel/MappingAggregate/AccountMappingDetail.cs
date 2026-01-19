using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.MappingAggregate;

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
}
