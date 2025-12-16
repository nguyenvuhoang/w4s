using System.Text.Json.Serialization;
using Newtonsoft.Json;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class VisibleTransactionResponse : BaseO24OpenAPIModel
{
    [JsonProperty("transaction_code")]
    [JsonPropertyName("transaction_code")]
    public string TransactionCode { get; set; }

    [JsonProperty("transaction_name")]
    [JsonPropertyName("transaction_name")]
    public string TransactionName { get; set; }

    [JsonProperty("transaction_name_language")]
    [JsonPropertyName("transaction_name_language")]
    public string TransactionNameLanguage { get; set; }

    [JsonProperty("module_code")]
    [JsonPropertyName("module_code")]
    public string ModuleCode { get; set; }
}
