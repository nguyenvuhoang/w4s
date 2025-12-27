using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.API.Application.Models;

public class Condition
{
    [JsonPropertyName("logic")]
    public string Logic { get; set; }

    [JsonPropertyName("conditions")]
    public List<Condition> Conditions { get; set; }

    [JsonPropertyName("field")]
    public string Field { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("value")]
    public object Value { get; set; }
}
