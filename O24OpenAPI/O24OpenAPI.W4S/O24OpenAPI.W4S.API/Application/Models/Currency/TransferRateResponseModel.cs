using Newtonsoft.Json;
using O24OpenAPI.Framework.Models;
using System.Text.Json.Serialization;

namespace O24OpenAPI.W4S.API.Application.Models.Currency
{

    public class TransferRateResponseModel : BaseTransactionModel
    {
        /// <summary>Mã tiền tệ (USD, EUR, JPY…)</summary>

        [JsonPropertyName("currency_code")]
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; } = default!;

        /// <summary>
        /// Transfer rate to BaseCurrency.
        /// Expected: amountBase = amount * Transfer
        /// </summary>
        [JsonPropertyName("transfer")]
        [JsonProperty("transfer")]
        public decimal? Transfer { get; set; }
    }
}
