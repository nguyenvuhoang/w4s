using System.Text.Json.Serialization;
using Newtonsoft.Json;
using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.O24ACT.Models;

public class FundTransferDetailModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Posting side
    /// </summary>
    [JsonProperty("posting_side")]
    [JsonPropertyName("posting_side")]
    public string PostingSide { get; set; }

    /// <summary>
    /// Account number
    /// </summary>
    [JsonProperty("account_number")]
    [JsonPropertyName("account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [JsonProperty("currency_code")]
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Value date
    /// </summary>
    [JsonProperty("value_date")]
    [JsonPropertyName("value_date")]
    public DateTime? ValueDate { get; set; }

    /// <summary>
    /// Group id
    /// </summary>
    [JsonProperty("group_id")]
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    /// <summary>
    /// Accounting group
    /// </summary>
    [JsonProperty("accounting_group")]
    [JsonPropertyName("accounting_group")]
    public int AccountingGroup { get; set; }

    /// <summary>
    /// Module code
    /// </summary>
    [JsonProperty("module_code")]
    [JsonPropertyName("module_code")]
    public string ModuleCode { get; set; }
}
