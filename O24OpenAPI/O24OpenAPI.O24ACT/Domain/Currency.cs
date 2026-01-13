using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

public partial class Currency : BaseEntity
{
    /// <summary>
    /// Currency
    /// </summary>
    public Currency() { }

    /// <summary>
    /// CurrencyId
    /// </summary>
    [JsonProperty("currency_id")]
    public string CurrencyId { get; set; }

    /// <summary>
    /// ShortCurrencyId
    /// </summary>
    [JsonProperty("short_currency_id")]
    public string ShortCurrencyId { get; set; }

    /// <summary>
    /// CurrencyName
    /// </summary>
    [JsonProperty("currency_name")]
    public string CurrencyName { get; set; }

    /// <summary>
    /// CurrencyNumber
    /// </summary>
    [JsonProperty("currency_number")]
    public int? CurrencyNumber { get; set; }

    /// <summary>
    /// MasterName
    /// </summary>
    [JsonProperty("master_name")]
    public string MasterName { get; set; }

    /// <summary>
    /// DecimalName
    /// </summary>
    [JsonProperty("decimal_name")]
    public string DecimalName { get; set; }

    /// <summary>
    /// DecimalDigits
    /// </summary>
    [JsonProperty("decimal_digits")]
    public int DecimalDigits { get; set; }

    /// <summary>
    /// RoundingDigits
    /// </summary>
    [JsonProperty("rounding_digits")]
    public int RoundingDigits { get; set; }

    /// <summary>
    /// StatusOfCurrency
    /// </summary>
    [JsonProperty("status_of_currency")]
    public string StatusOfCurrency { get; set; }

    /// <summary>
    /// Order
    /// </summary>
    [JsonProperty("display_order")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
