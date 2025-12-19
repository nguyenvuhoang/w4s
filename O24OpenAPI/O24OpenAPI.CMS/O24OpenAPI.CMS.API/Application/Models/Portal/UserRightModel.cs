using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Models.Portal;

public class UserRightModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// roleid
    /// </summary>
    [JsonProperty("roleid")]
    [JsonPropertyName("roleid")]
    public int RoleId { get; set; }

    /// <summary>
    /// cmdid
    /// </summary>
    [JsonProperty("commandid")]
    [JsonPropertyName("commandid")]
    public string CommandId { get; set; }

    /// <summary>
    /// cmdiddt
    /// </summary>
    [JsonProperty("commandiddetail")]
    [JsonPropertyName("commandiddetail")]
    public string CommandIdDetail { get; set; }

    /// <summary>
    /// invoke
    /// </summary>
    [JsonProperty("invoke")]
    [JsonPropertyName("invoke")]
    public int Invoke { get; set; }

    /// <summary>
    /// approve
    /// </summary>
    [JsonProperty("approve")]
    [JsonPropertyName("approve")]
    public int Approve { get; set; }
}
