using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

public partial class ForeignExchangeAccountDefinition : BaseEntity
{
    /// <summary>
    /// BranchCode
    /// </summary>
    [JsonProperty("branch_code")]
    public string BranchCode { get; set; }

    /// <summary>
    /// AccountCurrency
    /// </summary>
    [JsonProperty("account_currency")]
    public string AccountCurrency { get; set; }

    /// <summary>
    /// ClearingCurrency
    /// </summary>
    [JsonProperty("clearing_currency")]
    public string ClearingCurrency { get; set; }

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
}
