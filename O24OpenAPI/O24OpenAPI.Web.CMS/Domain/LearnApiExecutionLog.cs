using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Domain;

public partial class LearnApiExecutionLog : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public string ExecuteId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    ///
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string WorkflowId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Input { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Output { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime CreateOn { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime FinishOn { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string WorkflowFunc { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string IdFieldName { get; set; }

    /// <summary>
    /// Sets or gets the value of the module
    /// </summary>
    [JsonProperty("module")]
    public string Module { set; get; } = "";

    /// <summary>
    /// Sets or gets the value of the module
    /// </summary>
    [JsonProperty("tx_code")]
    public string TxCode { set; get; } = "";

    /// <summary>
    ///
    /// </summary>
    public UserSessions user_sessions = new();

    /// <summary>
    ///
    /// </summary>
    public UserSessions user_approve = new();
}
