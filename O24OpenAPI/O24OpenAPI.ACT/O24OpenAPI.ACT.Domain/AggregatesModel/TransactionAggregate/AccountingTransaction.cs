using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

/// <summary>
/// AccountingTransaction
/// </summary>
public partial class AccountingTransaction : BaseEntity
{
    /// <summary>
    /// AccountCommon
    /// </summary>
    public AccountingTransaction() { }

    /// <summary>
    /// Txrefid
    /// </summary>
    [JsonProperty("reference_id")]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Txdt
    /// </summary>
    [JsonProperty("transaction_date")]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// UserDefined1
    /// </summary>
    [JsonProperty("user_defined1")]
    public string UserDefined1 { get; set; }

    /// <summary>
    /// UserDefined2
    /// </summary>
    [JsonProperty("user_defined2")]
    public string UserDefined2 { get; set; }

    /// <summary>
    /// UserDefined3
    /// </summary>
    [JsonProperty("user_defined3")]
    public string UserDefined3 { get; set; }

    /// <summary>
    /// UserDefined4
    /// </summary>
    [JsonProperty("user_defined4")]
    public string UserDefined4 { get; set; }

    /// <summary>
    /// UserDefined5
    /// </summary>
    [JsonProperty("user_defined5")]
    public string UserDefined5 { get; set; }
}
