using System.Text.Json.Serialization;

namespace O24OpenAPI.WFO.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnumWorkflowType
{
    normal,
    reversal,
    approval,
}
