namespace O24OpenAPI.O24OpenAPIClient.Events;

/// <summary>
/// The generic event data class
/// </summary>
public class GenericEventData<TNeptuneEventData>
{
    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    public TNeptuneEventData data { get; set; }
}
