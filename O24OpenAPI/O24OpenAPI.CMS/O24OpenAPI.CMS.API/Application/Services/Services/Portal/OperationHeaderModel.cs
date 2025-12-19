using Newtonsoft.Json;

namespace O24OpenAPI.CMS.API.Application.Services.Services.Portal;

public class OperationHeaderModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public OperationHeaderModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("cmdid")]
    public string cmdid { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    [JsonProperty("caption")]
    public string caption { get; set; }
}
