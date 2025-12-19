namespace O24OpenAPI.CMS.Domain;

/// <summary>
/// The request log class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class HttpLogMessage : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the begin time
    /// </summary>
    public DateTimeOffset BeginTime { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the value of the begin time
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Sets or gets the value of the request body
    /// </summary>
    public string RequestBody { set; get; }

    /// <summary>
    /// Sets or gets the value of the reuest header
    /// </summary>
    public string RequestHeader { set; get; }

    /// <summary>
    /// Sets or gets the value of the response body
    /// </summary>
    public string ResponseBody { set; get; }
}
