using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The workflow service interface
/// </summary>

public interface IWorkflowService
{
    /// <summary>
    /// Executes the workflow step using the specified workflow
    /// </summary>
    /// <param name="learnApi">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> ExecuteWorkflowStep(LearnApiRequestModel learnApi);

    /// <summary>
    /// Updates the workflow step
    /// </summary>
    /// <param name="workflowStep">The workflow step</param>
    Task<WorkflowStep> Update(WorkflowStep workflowStep);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the workflow step</returns>
    Task<WorkflowStep> DeleteById(int id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of workflow step model</returns>
    Task<List<WorkflowStepModel>> GetAll();

    /// <summary>
    /// Gets the by app and id using the specified app
    /// </summary>
    /// <param name="app">The app</param>
    /// <param name="workflowId">The workflow id</param>
    /// <returns>A task containing the workflow step model</returns>
    Task<WorkflowStepModel> GetByAppAndId(string app, string workflowId);

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the workflow step</returns>
    Task<WorkflowStep> GetById(int id);

    /// <summary>
    /// Inserts the workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow step</returns>
    Task<WorkflowStep> Insert(WorkflowStep workflow);
}
