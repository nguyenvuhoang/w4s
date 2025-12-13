using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Neptune;

public class ResponseModel
{
    [JsonProperty("time_in_milliseconds")]
    public long TimeInMilliseconds { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("error_code")]
    public string ErrorCode { get; set; }

    [JsonProperty("execution_id")]
    public string ExecutionId { get; set; }

    [JsonProperty("transaction_number")]
    public object TransactionNumber { get; set; }

    [JsonProperty("transaction_date")]
    public object TransactionDate { get; set; }

    [JsonProperty("value_date")]
    public object ValueDate { get; set; }

    [JsonProperty("data")]
    public Dictionary<string, object> Data { get; set; } = [];
}
