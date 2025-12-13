namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class UploadResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public UploadResponseModel() { }
    /// <summary>
    /// </summary>
    public string user_id { get; set; } = null;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string name { get; set; } = null;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int status { get; set; } = -1;
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int statusCode { get; set; } = 200;


}
