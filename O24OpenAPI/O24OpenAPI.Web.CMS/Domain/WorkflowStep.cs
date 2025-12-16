using Newtonsoft.Json;
using O24OpenAPI.Client.Enums;

namespace O24OpenAPI.Web.CMS.Domain;

public class WorkflowStep : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the workflow id
    /// </summary>
    [JsonProperty("WFID")]
    public string WFId { get; set; }

    /// <summary>
    /// Gets or sets the value of the step order
    /// </summary>
    [JsonProperty("STEP_ORDER")]
    public int StepOrder { get; set; }

    /// <summary>
    /// Gets or sets the value of the step code
    /// </summary>
    [JsonProperty("STEP_CODE")]
    public string StepCode { get; set; }

    [JsonProperty("SERVICE_ID")]
    public string ServiceID { get; set; }

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    [JsonProperty("STATUS")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    [JsonProperty("DESCRIPTION")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    [JsonProperty("APP_CODE")]
    public string AppCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the sending template
    /// </summary>
    [JsonProperty("SENDING_TEMPLATE")]
    public string SendingTemplate { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the sending template
    /// </summary>
    [JsonProperty("MAPPING_RESPONSE")]
    public string MappingResponse { get; set; }

    /// <summary>
    /// Gets or sets the value of the request protocol
    /// </summary>
    [JsonProperty("REQUEST_PROTOCOL")]
    public string RequestProtocol { get; set; }

    /// <summary>
    /// Gets or sets the value of the request server ip
    /// </summary>
    [JsonProperty("REQUEST_SERVER_IP")]
    public string RequestServerIp { get; set; }

    /// <summary>
    /// Gets or sets the value of the request server port
    /// </summary>
    [JsonProperty("REQUEST_SERVER_PORT")]
    public string RequestServerPort { get; set; }

    /// <summary>
    /// Gets or sets the value of the request uri
    /// </summary>
    [JsonProperty("REQUEST_URI")]
    public string RequestUri { get; set; }

    /// <summary>
    /// Gets or sets the value of the step timeout
    /// </summary>
    [JsonProperty("STEP_TIMEOUT")]
    public long StepTimeout { get; set; } = 60000;

    /// <summary>
    /// Gets or sets the value of the sending condition
    /// </summary>
    [JsonProperty("SENDING_CONDITION")]
    public string SendingCondition { get; set; } = "{}";

    /// <summary>
    /// Gets or sets the value of the processing number
    /// </summary>
    [JsonProperty("PROCESSING_NUMBER")]
    public ProcessNumber ProcessingNumber { get; set; } = ProcessNumber.Version1;

    /// <summary>
    /// Gets or sets the value of the full class name
    /// </summary>
    [JsonProperty("FULLCLASSNAME")]
    public string FullClassName { get; set; }

    /// <summary>
    /// Gets or sets the value of the methodname
    /// </summary>
    [JsonProperty("METHODNAME")]
    public string MethodName { get; set; }

    public bool IsReverse { get; set; } = false;

    public bool ShouldAwaitStep { get; set; } = true;
}
