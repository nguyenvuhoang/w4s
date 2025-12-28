using System.Text.Json.Serialization;

namespace O24OpenAPI.NCH.Models;

public class SMSProviderConfigModel
{
    [JsonPropertyName("configKey")]
    public string ConfigKey { get; set; }

    [JsonPropertyName("configValue")]
    public string ConfigValue { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("isactive")]
    public bool IsActive { get; set; }
    [JsonPropertyName("action")]
    public string Action { get; set; }
}
