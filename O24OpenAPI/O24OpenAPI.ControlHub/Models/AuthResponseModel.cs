using Newtonsoft.Json;
using O24OpenAPI.Web.Framework.Models;
using System.Text.Json.Serialization;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The auth response model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class AuthResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the token
    /// </summary>
    [JsonProperty("token")]
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the value of the refresh token
    /// </summary>
    [JsonProperty("refresh_token")]
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonProperty("expired_in")]
    [JsonPropertyName("expired_in")]
    public DateTime ExpiredIn { get; set; }
    [JsonProperty("expired_duration")]
    [JsonPropertyName("expired_duration")]
    public long ExpiredDuration { get; set; }
    [JsonProperty("is_first_login")]
    [JsonPropertyName("is_first_login")]
    public bool IsFirstLogin { get; set; } = true;
}
