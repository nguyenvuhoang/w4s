namespace O24OpenAPI.O24OpenAPIClient.Log;

/// <summary>
/// The centralized log class
/// </summary>
public class CentralizedLog
{
    /// <summary>
    /// Gets or sets the value of the log utc
    /// </summary>
    public long log_utc { get; set; }

    /// <summary>
    /// Gets or sets the value of the log type
    /// </summary>
    public string log_type { get; set; }

    /// <summary>
    /// Gets or sets the value of the execution id
    /// </summary>
    public string execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the step execution id
    /// </summary>
    public string step_execution_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the step code
    /// </summary>
    public string step_code { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    public string service_id { get; set; }

    /// <summary>
    /// Gets or sets the value of the subject
    /// </summary>
    public string subject { get; set; }

    /// <summary>
    /// Gets or sets the value of the log text
    /// </summary>
    public string log_text { get; set; }

    /// <summary>
    /// Gets or sets the value of the json details
    /// </summary>
    public string json_details { get; set; }
}
