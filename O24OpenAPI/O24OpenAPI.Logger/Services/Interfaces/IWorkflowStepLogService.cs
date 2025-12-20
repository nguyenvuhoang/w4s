using O24OpenAPI.Client.Scheme.Workflow;

namespace O24OpenAPI.Logger.Services.Interfaces;

/// <summary>
///
/// </summary>
public interface IWorkflowStepLogService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme);
}
