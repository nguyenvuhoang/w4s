using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class GetListYearResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public GetListYearResponseModel() => CodeName = "YEAR";

    /// <summary>
    /// CodeName
    /// </summary>
    /// <value></value>
    public string CodeName { get; set; }

    /// <summary>
    /// Caption
    /// </summary>
    /// <value></value>
    public int Caption { get; set; }
}
