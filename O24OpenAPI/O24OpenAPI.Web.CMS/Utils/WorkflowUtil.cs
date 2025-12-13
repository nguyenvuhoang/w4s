using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Utils;

public static class WorkflowUtil
{
    public static WorkflowRequestModel ToO9Model(
        this WorkflowScheme workflowScheme,
        UserSessions userSession = null
    )
    {
        var model = JsonConvert.DeserializeObject<WorkflowRequestModel>(
            ((JsonElement)workflowScheme.Request.RequestBody.Data).GetRawText()
        );
        if (userSession == null)
        {
            model.user_sessions = SessionUtils.GetUserSession();
        }
        else
        {
            model.user_sessions = userSession;
        }

        model.ObjectField = JObject.Parse(JsonConvert.SerializeObject(model.fields));
        model.MappingResponse ??=
            workflowScheme.Request.RequestHeader.TxContext["step_mapping_response"]?.ToString()
            ?? workflowScheme
                .Request.RequestHeader.TxContext["learnapi_mapping_response"]
                ?.ToString();
        return model;
    }

    public static string GetStringTxdt(this WorkflowRequestModel workflow)
    {
        var jToken = workflow.ObjectField["B"];
        DateTime dt = jToken.Value<DateTime>();
        string txdt = dt.ToString("dd/MM/yyyy HH:mm:ss");
        txdt = txdt.Replace("-", "/");
        return txdt;
    }

    /// <summary>
    /// Invokes a workflow action and processes the result with mapping by default.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the action.</typeparam>
    /// <param name="workflow">The workflow request model.</param>
    /// <param name="action">The action to be invoked.</param>
    /// <param name="isBuildResponse">Indicates whether to build a workflow response.</param>
    /// <param name="isBuildWithMapping"></param>
    /// <returns>A JToken representing the processed result or exception.</returns>
    public static Task<JToken> Invoke<T>(
        this WorkflowRequestModel workflow,
        Func<Task<T>> action,
        bool isBuildResponse = true,
        bool isBuildWithMapping = true
    )
    {
        return InvokeInternal(workflow, action, isBuildResponse, isBuildWithMapping);
    }

    /// <summary>
    /// Invokes a workflow action and processes the result without mapping by default.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the action.</typeparam>
    /// <param name="workflow">The workflow request model.</param>
    /// <param name="action">The action to be invoked.</param>
    /// <param name="isBuildResponse">Indicates whether to build a workflow response.</param>
    /// <param name="isBuildWithMapping"></param>
    /// <returns>A JToken representing the processed result or exception.</returns>
    public static Task<JToken> Invoke1<T>(
        this WorkflowRequestModel workflow,
        Func<Task<T>> action,
        bool isBuildResponse = true,
        bool isBuildWithMapping = false
    )
    {
        return InvokeInternal(workflow, action, isBuildResponse, isBuildWithMapping);
    }

    private static async Task<JToken> InvokeInternal<T>(
        WorkflowRequestModel workflow,
        Func<Task<T>> action,
        bool isBuildResponse,
        bool isBuildWithMapping
    )
    {
        try
        {
            var result = await action();
            var response = result switch
            {
                JToken jToken => jToken,
                null => throw new Exception(
                    $"Result is null from executing the workflow [{workflow.workflowid}]!"
                ),
                _ => JToken.FromObject(result),
            };

            return isBuildResponse
                ? response.BuildWorkflowResponseSuccess(
                    isBuildWithMapping,
                    build_from: "O9WORKFLOW"
                )
                : response;
        }
        catch (Exception ex)
        {
            return await ex.HandleExceptionAsync(workflow.WorkflowFunc, workflow.workflowid);
        }
    }
}
