using System.Text.Json.Serialization;

namespace O24OpenAPI.Web.CMS.Models;

public class CoreAPIKeyModel : BaseO24OpenAPIModel
{
    [JsonPropertyName("clientid")]
    public string ClientId { get; set; }
    [JsonPropertyName("clientsecret")]
    public string ClientSecret { get; set; }
    [JsonPropertyName("displayname")]
    public string DisplayName { get; set; }
    [JsonPropertyName("environment")]
    public string Environment { get; set; }
    [JsonPropertyName("scopes")]
    public string Scopes { get; set; }

    [JsonPropertyName("expiredonutc")]
    public DateTime ExpiredOnUtc { get; set; }
    [JsonPropertyName("isrevoked")]
    public bool IsRevoked { get; set; } = false;

    [JsonPropertyName("isactive")]
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("createdonutc")]
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("createdby")]
    public string CreatedBy { get; set; }
    [JsonPropertyName("lastusedonutc")]
    public DateTime? LastUsedOnUtc { get; set; }
    [JsonPropertyName("usagecount")]
    public int UsageCount { get; set; } = 0;
}
