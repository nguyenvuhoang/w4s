using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.MappingAggregate;

//D_ACMAP
/// <summary>
/// AccountMapping
/// </summary>
public partial class AccountMapping : BaseEntity
{
    /// <summary>
    /// MappingId
    /// </summary>
    [JsonProperty("mapping_id")]
    public string? MappingId { get; set; }

    /// <summary>
    /// MappingTableName
    /// </summary>
    [JsonProperty("mapping_table_name")]
    public string? MappingTableName { get; set; }

    /// <summary>
    /// MappingType
    /// </summary>
    [JsonProperty("mapping_type")]
    public string? MappingType { get; set; }

    /// <summary>
    /// AccountName
    /// </summary>
    [JsonProperty("account_name")]
    public string? AccountName { get; set; }
}
