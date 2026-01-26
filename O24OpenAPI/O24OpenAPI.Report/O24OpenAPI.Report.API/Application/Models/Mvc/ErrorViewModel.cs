namespace O24OpenAPI.Report.API.Application.Models.Mvc;

public class ErrorViewModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string RequestId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Message { get; set; }
    /// <summary>
    /// ErrorCode
    /// </summary>
    /// <value>404/401/500</value>
    public int ErrorCode { get; set; }
}
