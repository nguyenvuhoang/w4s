using Newtonsoft.Json;
using O24OpenAPI.Client.Enums;

namespace O24OpenAPI.Web.CMS.Models;

public partial class WorkflowStepModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the  id
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    [JsonProperty("STATUS")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    [JsonProperty("DESCRIPTION")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the service id
    /// </summary>
    [JsonProperty("SERVICE_ID")]
    public string ServiceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the sending template
    /// </summary>
    [JsonProperty("SENDING_TEMPLATE")]
    public string SendingTemplate { get; set; }

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
    public long StepTimeout { get; set; }

    /// <summary>
    /// Gets or sets the value of the sending condition
    /// </summary>
    [JsonProperty("SENDING_CONDITION")]
    public string SendingCondition { get; set; }

    /// <summary>
    /// Gets or sets the value of the processing number
    /// </summary>
    [JsonProperty("PROCESSING_NUMBER")]
    public ProcessNumber ProcessingNumber { get; set; }

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

    public bool IsReverse { get; set; }
}

public class WorkflowStepSearchModel : SimpleSearchModel
{
    public string WFId { get; set; }
    public string StepCode { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}

public class WorkflowStepSearchResponse : BaseO24OpenAPIModel
{
    [JsonProperty("wfid")]
    public string WFId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}
