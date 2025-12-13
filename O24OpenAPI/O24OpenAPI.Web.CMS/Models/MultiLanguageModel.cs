using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class MultiLanguageModel : BaseO24OpenAPIModel
{
    [JsonProperty("en")]
    public string English { get; set; }

    [JsonProperty("vi")]
    public string Vietnamese { get; set; }

    [JsonProperty("lo")]
    public string Lao { get; set; }

    [JsonProperty("zh")]
    public string Chinese { get; set; }
}
