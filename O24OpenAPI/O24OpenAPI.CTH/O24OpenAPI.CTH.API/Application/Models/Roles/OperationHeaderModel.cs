using Newtonsoft.Json;
using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

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
