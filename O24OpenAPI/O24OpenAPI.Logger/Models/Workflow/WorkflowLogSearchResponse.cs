using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Models.Workflow;

/// <summary>
/// The http log search response class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class WorkflowLogSearchResponse : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the input
    /// </summary>
    public string input { get; set; }

    /// <summary>
    /// Gets or sets the value of the workflow id
    /// </summary>
    public string workflow_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public int status { get; set; }
    /// <summary>
    /// Gets or sets the value of the error
    /// </summary>
    public string error { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on
    /// </summary>
    public long created_on { get; set; }

    /// <summary>
    /// Gets or sets the value of the finish on
    /// </summary>
    public long finish_on { get; set; }

    /// <summary>
    /// Gets or sets the value of the is timeout
    /// </summary>
    public string is_timeout { get; set; }

    /// <summary>
    /// Gets or sets the value of the is processing
    /// </summary>
    public string is_processing { get; set; }

    /// <summary>
    /// Gets or sets the value of the is success
    /// </summary>
    public string is_success { get; set; }

    /// <summary>
    /// Gets or sets the value of the workflow type
    /// </summary>
    public string workflow_type { get; set; }
    /// <summary>
    /// Gets or sets the value of the response content
    /// </summary>
    public string response_content { get; set; }

    /// <summary>
    /// Gets or sets the value of the reversed execution id
    /// </summary>
    public string reversed_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the reversed by execution id
    /// </summary>
    public string reversed_by_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the is disputed
    /// </summary>
    public string is_disputed { get; set; }

    /// <summary>
    /// Gets or sets the value of the archiving time
    /// </summary>
    public long archiving_time { get; set; }

    /// <summary>
    /// Gets or sets the value of the purging time
    /// </summary>
    public long purging_time { get; set; }

    /// <summary>
    /// Gets or sets the value of the approved execution id
    /// </summary>
    public string approved_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the transaction number
    /// </summary>
    public string transaction_number { get; set; }

    /// <summary>
    /// Gets or sets the value of the transaction date
    /// </summary>
    public string transaction_date { get; set; }

    /// <summary>
    /// Gets or sets the value of the value date
    /// </summary>
    public string value_date { get; set; }
}
