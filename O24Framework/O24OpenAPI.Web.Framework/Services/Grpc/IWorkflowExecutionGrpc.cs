using O24OpenAPI.O24OpenAPIClient.Workflow;

namespace O24OpenAPI.Web.Framework.Services.Grpc;

/// <summary>
/// The workflow execution grpc interface
/// </summary>
public interface IWorkflowExecutionGrpc
{
    /// <summary>
    /// Executes the workflow by grpc using the specified input
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns>A task containing the workflow response</returns>
    Task<WorkflowResponse> ExecuteWorkflowByGrpc(string input);
}
