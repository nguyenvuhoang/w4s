using System.Text.Json.Serialization;

namespace O24OpenAPI.CMS.API.Application.Models;

public class BoRequestModel : BaseO24OpenAPIModel
{
    public BoRequestModel() { }

    [JsonPropertyName("bo")]
    public List<BoRequest> Bo { get; set; } = [];
}

public class BoRequest : BaseO24OpenAPIModel
{
    [JsonPropertyName("input")]
    public Dictionary<string, object> Input { get; set; } = [];
}
