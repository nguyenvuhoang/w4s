namespace O24OpenAPI.Web.CMS.Models.Mvc;

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
}
