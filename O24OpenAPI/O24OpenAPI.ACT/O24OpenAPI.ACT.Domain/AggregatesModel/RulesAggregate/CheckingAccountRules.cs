using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

public partial class CheckingAccountRules : BaseEntity
{
    /// <summary>
    /// AccountClassification
    /// </summary>
    [JsonProperty("account_classification")]
    public string? AccountClassification { get; set; }

    /// <summary>
    /// ReverseBalance
    /// </summary>
    [JsonProperty("reverse_balance")]
    public string? ReverseBalance { get; set; }

    /// <summary>
    /// BalanceSide
    /// </summary>
    [JsonProperty("balance_side")]
    public string? BalanceSide { get; set; }

    /// <summary>
    /// PostingSide
    /// </summary>
    [JsonProperty("posting_side")]
    public string? PostingSide { get; set; }

    /// <summary>
    /// AccountGroup
    /// </summary>
    [JsonProperty("account_group")]
    public string? AccountGroup { get; set; }

    /// <summary>
    /// AccountCategories
    /// </summary>
    [JsonProperty("account_categories")]
    public string? AccountCategories { get; set; }
}
