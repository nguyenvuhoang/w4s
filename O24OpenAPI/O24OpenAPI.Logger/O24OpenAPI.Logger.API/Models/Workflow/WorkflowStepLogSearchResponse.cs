using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Logger.API.Models.Workflow;

/// <summary>
/// The http log search response class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class WorkflowStepLogSearchResponse : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the step execution id
    /// </summary>
    public string step_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the step order
    /// </summary>
    public int step_order { get; set; }

    /// <summary>
    /// Gets or sets the value of the step code
    /// </summary>
    public string step_code { get; set; }

    /// <summary>
    /// Gets or sets the value of the sending condition
    /// </summary>
    public string sending_condition { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 request
    /// </summary>
    public string p1_request { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 start
    /// </summary>
    public long p1_start { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 finish
    /// </summary>
    public long p1_finish { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 status
    /// </summary>
    public int p1_status { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 error
    /// </summary>
    public string p1_error { get; set; }

    /// <summary>
    /// Gets or sets the value of the p1 content
    /// </summary>
    public string p1_content { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 request
    /// </summary>
    public string p2_request { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 start
    /// </summary>
    public long p2_start { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 finish
    /// </summary>
    public long p2_finish { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 status
    /// </summary>
    public int p2_status { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 error
    /// </summary>
    public string p2_error { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 error code
    /// </summary>
    public string p2_error_code { get; set; }

    /// <summary>
    /// Gets or sets the value of the p2 content
    /// </summary>
    public string p2_content { get; set; }

    /// <summary>
    /// Gets or sets the value of the is success
    /// </summary>
    public string is_success { get; set; }

    /// <summary>
    /// Gets or sets the value of the is timeout
    /// </summary>
    public string is_timeout { get; set; }
}
