using O24OpenAPI.APIContracts.Models.DTS;
using System.Text.Json.Serialization;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UserResponseModel
{
    public object DataTemplate { get; set; }
    public List<DTSMimeEntityModel> MimeEntities { get; set; } = [];
}
public class MimeEntity
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
