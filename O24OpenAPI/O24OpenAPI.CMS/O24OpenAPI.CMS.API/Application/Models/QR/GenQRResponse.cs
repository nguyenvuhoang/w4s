using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models.QR;

public class GenQRResponse : BaseO24OpenAPIModel
{
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
