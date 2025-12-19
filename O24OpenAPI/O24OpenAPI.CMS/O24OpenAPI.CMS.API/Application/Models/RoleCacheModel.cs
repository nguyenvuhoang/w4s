namespace O24OpenAPI.CMS.API.Application.Models;

public class RoleCacheModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public RoleCacheModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string FormId { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool Invoke { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool Approve { get; set; } = true;
}
