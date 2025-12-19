using System.Text.Json.Serialization;
using Newtonsoft.Json;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Logger.Models.ServiceLog;

/// <summary>
/// The service log search response class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class ServiceLogSearchResponse : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the log level id
    /// </summary>
    [JsonPropertyName("log_level_id")]
    [JsonProperty("log_level_id")]
    public int LogLevelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    [JsonPropertyName("service_id")]
    [JsonProperty("service_id")]
    public string ServiceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    [JsonPropertyName("channel_id")]
    [JsonProperty("channel_id")]
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    [JsonPropertyName("status")]
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the short message
    /// </summary>
    [JsonPropertyName("short_message")]
    [JsonProperty("short_message")]
    public string ShortMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the full message
    /// </summary>
    [JsonPropertyName("full_message")]
    [JsonProperty("full_message")]
    public string FullMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    [JsonPropertyName("data")]
    [JsonProperty("data")]
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    [JsonPropertyName("user_id")]
    [JsonProperty("user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    [JsonPropertyName("reference")]
    [JsonProperty("reference")]
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    [JsonPropertyName("ip_address")]
    [JsonProperty("ip_address")]
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the value of the user agent
    /// </summary>
    [JsonPropertyName("user_agent")]
    [JsonProperty("user_agent")]
    public string UserAgent { get; set; }
}
