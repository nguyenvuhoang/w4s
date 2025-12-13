using System.Text.Json.Serialization;
using Newtonsoft.Json;
using O24OpenAPI.Web.CMS.Constant;

namespace O24OpenAPI.Web.CMS.Models;

public class StatusResponse(string status, string message = "")
{
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public string Status { get; set; } = status;

    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; } = message;
}

public class StatusCompleteResponse(string message = "")
{
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public string Status { get; set; } = TranStatus.COMPLETED;

    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; } = message;
}
