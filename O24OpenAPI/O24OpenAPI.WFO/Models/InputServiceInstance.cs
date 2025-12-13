using Newtonsoft.Json;

namespace O24OpenAPI.WFO.Models;

public class InputServiceInstance
{
    [JsonProperty("service_id")]
    public string ServiceId { get; set; }

    [JsonProperty("instance_id")]
    public string InstanceId { get; set; }
}
