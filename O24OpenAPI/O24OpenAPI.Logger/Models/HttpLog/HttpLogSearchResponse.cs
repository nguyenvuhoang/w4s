using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Models.HttpLog;

/// <summary>
/// The http log search response class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class HttpLogSearchResponse : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the request id
    /// </summary>
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or sets the value of the http method
    /// </summary>
    public string HttpMethod { get; set; }

    /// <summary>
    /// Gets or sets the value of the request url
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// Gets or sets the value of the request headers
    /// </summary>
    public string RequestHeaders { get; set; }

    /// <summary>
    /// Gets or sets the value of the request body
    /// </summary>
    public new string RequestBody { get; set; }

    /// <summary>
    /// Gets or sets the value of the client ip
    /// </summary>
    public string ClientIp { get; set; }

    /// <summary>
    /// Gets or sets the value of the user agent
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the value of the response status code
    /// </summary>
    public int ResponseStatusCode { get; set; } = 0;

    /// <summary>
    /// Gets or sets the value of the response headers
    /// </summary>
    public string ResponseHeaders { get; set; }

    /// <summary>
    /// Gets or sets the value of the response body
    /// </summary>
    public string ResponseBody { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    public string ServiceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the exception message
    /// </summary>
    public string ExceptionMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the stack trace
    /// </summary>
    public string StackTrace { get; set; }
}
