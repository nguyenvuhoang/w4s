namespace O24OpenAPI.Web.CMS.Models.RabbitMQ;

using System.Text.Json.Serialization;

/// <summary>
/// Defines the <see cref="RabbitQueueDetail" />
/// </summary>
public class RabbitQueueDetail
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Consumers
    /// </summary>
    [JsonPropertyName("consumers")]
    public int Consumers { get; set; }

    /// <summary>
    /// Gets or sets the ConsumerDetails
    /// </summary>
    [JsonPropertyName("consumer_details")]
    public List<ConsumerDetail> ConsumerDetails { get; set; } = [];
}

/// <summary>
/// Defines the <see cref="ConsumerDetail" />
/// </summary>
public class ConsumerDetail
{
    /// <summary>
    /// Gets or sets the ConsumerTag
    /// </summary>
    [JsonPropertyName("consumer_tag")]
    public string ConsumerTag { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether Active
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
