namespace O24OpenAPI.Web.CMS.Domain;

/// <summary>
/// The workflow step log class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class WorkflowStepLog : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string ExecutionId { get; set; }

    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string CacheExecutionId { get; set; }

    /// <summary>
    /// Gets or sets the value of the step execution id
    /// </summary>
    public string StepExecutionId { get; set; }

    /// <summary>
    /// Gets or sets the value of the step code
    /// </summary>
    public string StepCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the tx context
    /// </summary>
    public string TxContextData { get; set; }

    /// <summary>
    /// Gets or sets the value of the request data
    /// </summary>
    public string RequestData { get; set; }

    /// <summary>
    /// Gets or sets the value of the response data
    /// </summary>
    public string ResponseData { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ProcessingVersion { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    public string ServiceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the server ip
    /// </summary>
    public string ServerIp { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the utc send time
    /// </summary>
    public long UtcSendTime { get; set; }

    /// <summary>
    /// Gets or sets the value of the process in
    /// </summary>
    public long ProcessIn { get; set; }

    /// <summary>
    /// Gets or sets the value of the reversal execution id
    /// </summary>
    public string ReversalExecutionId { get; set; }

    /// <summary>
    /// Gets or sets the value of the approval execution id
    /// </summary>
    public string ApprovalExecutionId { get; set; }

    public string WorkflowScheme { get; set; }
}
