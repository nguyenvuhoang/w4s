namespace O24OpenAPI.Web.Framework.Models;

/// <summary>
/// The wf class
/// </summary>
public class WF<T>
{
    /// <summary>
    /// Gets or sets the value of the request
    /// </summary>
    public WFRequest<T> request { get; set; }

    /// <summary>
    /// Gets or sets the value of the response
    /// </summary>
    public Response response { get; set; }
}
