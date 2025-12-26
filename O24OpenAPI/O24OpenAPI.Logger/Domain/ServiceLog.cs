using System.Text.Json.Serialization;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Logger.Domain;

/// <summary>
/// The service log class
/// </summary>
/// <seealso cref="BaseEntity"/>
public partial class ServiceLog : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the execution log id
    /// </summary>
    [JsonPropertyName("execution_log_id")]
    public string ExecutionLogId { get; set; }

    /// <summary>
    /// Gets or sets the value of the log level id
    /// </summary>
    [JsonPropertyName("log_level_id")]
    public int LogLevelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    [JsonPropertyName("service_id")]
    public string ServiceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the value of the short message
    /// </summary>
    [JsonPropertyName("short_message")]
    public string ShortMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the full message
    /// </summary>
    [JsonPropertyName("full_message")]
    public string FullMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    [JsonPropertyName("ip_address")]
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the value of the user agent
    /// </summary>
    [JsonPropertyName("user_agent")]
    public string UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
