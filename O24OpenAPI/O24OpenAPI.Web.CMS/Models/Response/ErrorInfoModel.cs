namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class ErrorInfoModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string key { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string type { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string info { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string typeError { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string executeId { get; set; } = string.Empty;

    public string next_action { get; set; } = string.Empty;
}
