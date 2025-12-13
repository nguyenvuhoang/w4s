using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models.ContextModels;

namespace O24OpenAPI.Web.CMS.Models;

public class WorkflowRequestModel : WorkflowExecuteModel
{
    [JsonProperty("workflow_execution_id")]
    public string WorkflowExecutionId { get; set; }

    [JsonProperty("workflow_func")]
    public string WorkflowFunc { get; set; }

    [JsonProperty("table_name")]
    public string TableName { get; set; }

    [JsonProperty("id_field_name")]
    public string IdFieldName { get; set; }

    [JsonProperty("action_type")]
    public string ActionType { get; set; }

    [JsonProperty("module")]
    public string Module { set; get; } = "";

    [JsonProperty("tx_code")]
    public string TxCode { set; get; } = "";

    [JsonProperty("tx_type")]
    public string TxType { get; set; }

    [JsonProperty("tx_ref_id")]
    public string TxRefId { set; get; } = "";

    public string MappingResponse { get; set; }

    public JObject ObjectField { get; set; }

    public string StringWorkingDate { get; set; }

    public DateTime? ValueDate { get; set; }

    public bool IsWorkflow { get; set; }

    public bool IsFrontOfficeMapping { get; set; } = true;

    public UserSessions user_sessions = new();

    public UserSessions user_approve = new();
}

public class WorkflowResponseModel
{
    /// <summary>
    ///
    /// </summary>
    public WorkflowResponseModel() { }

    /// <summary>
    ///
    /// </summary>
    public WorkflowResponseModel(
        int _status,
        string _error_message,
        string errorCode,
        JToken _result = null,
        bool _needs_mapping = false,
        string _build_from = null
    )
    {
        var context = EngineContext.Current.Resolve<JWebUIObjectContextModel>();
        result = _result;
        error_message = _error_message;
        status = _status;
        needs_mapping = _needs_mapping;
        error_code = errorCode;
        executeId = context.InfoRequest.ExecuteId;
        build_from = _build_from;
    }

    public string executeId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JToken result { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string error_message { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string error_code { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool needs_mapping { get; set; }

    public string build_from { get; set; }
}

public class WorkflowExecuteModel
{
    /// <summary>
    ///
    /// </summary>
    public enum EnumSupportLanguages
    {
        /// <summary>
        ///
        /// </summary>
        en,

        ///
        vi,

        /// <summary>
        ///
        /// </summary>
        la,

        /// <summary>
        ///
        /// </summary>
        kr,

        /// <summary>
        ///
        /// </summary>
        ///
        mm,

        /// <summary>
        ///
        /// </summary>
        ///
        th,
    }

    /// <summary>
    ///
    /// </summary>
    public WorkflowExecuteModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int RequestId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string workflowid = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public string description = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string lang { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int workflow_type = 0;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string reversal_execution_id = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string approved_execution_id = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string token = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public Dictionary<string, object> fields = new Dictionary<string, object>();

    /// <summary>
    ///
    /// </summary>
    public string reference_id = System.Guid.NewGuid().ToString();
}

public class WorkflowInfo
{
    /// <summary>
    ///
    /// </summary>
    public Func<WorkflowExecuteModel, Task<JToken>> InvokeWorkflow { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public int IsCommonProcess { get; set; } = 1;

    /// <summary>
    ///
    /// </summary>
    public string MappingRequest { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string MappingResponse { get; set; }

    /// <summary>
    ///
    /// </summary>
    public BoInfos BoInfo { get; set; } = null;

    public class BoInfos
    {
        /// <summary>
        ///
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string WorkflowFunc { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string IdFieldName { get; set; }
    }
}

public static class BuildWorkflowResponse
{
    private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new LowercaseNamingStrategy(),
        },
    };

    /// <summary>
    ///
    /// </summary>
    public static JObject BuildWorkflowResponseSuccess(
        this object value,
        bool needs_mapping = true,
        string error_message = null,
        string build_from = null
    )
    {
        ArgumentNullException.ThrowIfNull(value);

        JToken token =
            value is JToken
                ? (JToken)value
                : JToken.FromObject(value, Newtonsoft.Json.JsonSerializer.Create(_settings));
        var rs = new WorkflowResponseModel(
            ExecutionStatus.SUCCESS,
            error_message,
            "",
            token,
            needs_mapping,
            build_from
        );
        return JObject.FromObject(rs, Newtonsoft.Json.JsonSerializer.Create(_settings));
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject BuildWorkflowResponseError(
        this string error_message,
        JToken result = null
    )
    {
        error_message ??= "An unexpected error occurred.";
        var rs = new WorkflowResponseModel(ExecutionStatus.ERROR, error_message, "", result);
        return rs.ToJObject();
    }

    public static JObject BuildWorkflowResponseError(
        this string error_message,
        string errorCode,
        JToken result = null
    )
    {
        error_message ??= "An unexpected error occurred.";
        var rs = new WorkflowResponseModel(ExecutionStatus.ERROR, error_message, errorCode, result);
        return rs.ToJObject();
    }
}

public class LowercaseNamingStrategy : NamingStrategy
{
    protected override string ResolvePropertyName(string name)
    {
        return name.ToLowerInvariant();
    }
}
