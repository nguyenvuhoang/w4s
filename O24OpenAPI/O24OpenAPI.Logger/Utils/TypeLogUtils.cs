using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.Logger.Utils;

/// <summary>
/// The type log utils class
/// </summary>
public class TypeLogUtils
{
    /// <summary>
    /// Gets the type command using the specified log type
    /// </summary>
    /// <param name="logType">The log type</param>
    /// <returns>The type</returns>
    public static Type GetTypeLog(string logType)
    {
        return logType switch
        {
            "HTTP_LOG" => typeof(HttpLog),
            "WORKFLOW_LOG" => typeof(WorkflowLog),
            "WORKFLOW_STEP_LOG" => typeof(WorkflowStepLog),
            _ => typeof(ServiceLog),
        };
    }
}
