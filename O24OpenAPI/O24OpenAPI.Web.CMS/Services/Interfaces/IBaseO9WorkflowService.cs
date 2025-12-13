using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IBaseO9WorkflowService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> AdvanceSearch(WorkflowRequestModel workflow);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> SimpleSearch(WorkflowRequestModel workflow);

    /// <summary>
    /// Rules the func using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> RuleFunc(WorkflowRequestModel workflow);

    /// <summary>
    /// Executes the rule func using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> ExecuteRuleFunc(WorkflowRequestModel workflow);

    /// <summary>
    /// Fronts the office using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> FrontOffice(WorkflowRequestModel workflow);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<JToken> View(WorkflowRequestModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<JToken> Delete(WorkflowRequestModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<JToken> CreateBO(WorkflowRequestModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<JToken> UpdateBO(WorkflowRequestModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> SearchList(WorkflowRequestModel workflow);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> SearchInfo(WorkflowRequestModel workflow);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> ExecuteQuery(WorkflowRequestModel workflow);

    /// <summary>
    /// Calls the function using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> CallFunction(WorkflowRequestModel model);
}
