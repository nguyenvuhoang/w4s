using System.Text.Json.Serialization;

namespace O24OpenAPI.NCH.API.Application.Models.Request;

public class O24MimeEntity
{
    /// <summary>
    /// Gets or sets the value of the content type
    /// </summary>
    [JsonPropertyName("content_type")]
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the value of the base 64
    /// </summary>
    [JsonPropertyName("base64")]
    public string Base64 { get; set; }

    /// <summary>
    /// Gets or sets the value of the content id
    /// </summary>
    [JsonPropertyName("content_id")]
    public string ContentId { get; set; }
}
