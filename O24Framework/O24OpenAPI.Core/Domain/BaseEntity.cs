using Newtonsoft.Json;

namespace O24OpenAPI.Core.Domain;

/// <summary>
/// The base entity class
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
