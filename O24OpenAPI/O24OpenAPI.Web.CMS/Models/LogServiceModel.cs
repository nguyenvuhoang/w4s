namespace O24OpenAPI.Web.CMS.Models;

public class LogServiceModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public LogServiceModel()
    {

    }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int Id { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public long LogUtc { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Date { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Time { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string LogType { get; set; }

    /// <summary>
    /// ExecutionID
    /// </summary>
    public string ExecutionId { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string StepExecutionId { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string StepCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string ServiceId { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Subject { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string LogText { get; set; }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string JsonDetails { get; set; } = "{}";
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public double RelativeDuration { get; set; } = 0;
}
