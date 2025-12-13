using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnumSupportLanguages
{
    en,
    vi,
    la,
    kr,
    mm,
    th,
}
