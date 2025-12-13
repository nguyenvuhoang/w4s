using System.Text.Json.Serialization;

namespace O24OpenAPI.Web.CMS.Models.Request;

/// <summary>
///
/// </summary>
public class BoMappingCoreAPIModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("bo")]
    public List<CoreAPIRequest> Bo { get; set; } = [];
}

public class CoreAPIRequest : BaseO24OpenAPIModel
{
    [JsonPropertyName("use_microservice")]
    public bool UseMicroservice { get; set; } = false;

    [JsonPropertyName("input")]
    public InputModel Input { get; set; }
}

public class InputModel : BaseO24OpenAPIModel
{
    [JsonPropertyName("workflowid")]
    public string WorkFlowId { get; set; }
    [JsonPropertyName("learn_api")]
    public string LearnApi { get; set; }
    [JsonPropertyName("fields")]
    public Dictionary<string, object> Fields { get; set; }
}
