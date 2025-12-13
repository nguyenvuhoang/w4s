using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace O24OpenAPI.Web.CMS.Models.QR;

public class GenQRResponse : BaseO24OpenAPIModel
{
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
