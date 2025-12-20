using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The bankworkflowservice class
/// </summary>
/// <seealso cref="BaseQueueService"/>
public class SecurityQuestionWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ISecurityQuestionService _cSecurityQuestion =
        EngineContext.Current.Resolve<ISecurityQuestionService>();

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var listData = await _cSecurityQuestion.GetAll();
                return listData;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SecurityQuestionInsertModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.Insert(
                    model.FromModel<D_SECURITY_QUESTION>()
                );
                var response = await _cSecurityQuestion.ViewById(result.Id);
                return response;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SecurityQuestionUpdateModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.Update(
                    model.FromModel<D_SECURITY_QUESTION>()
                );
                var response = await _cSecurityQuestion.ViewById(result.Id);
                return response;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.DeleteById(model.Id);
                return result;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> SearchSimple(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.SimpleSearch(model);
                return result;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> SearchAdvance(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SecurityQuestionSearchAdvanceModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.SearchAdvance(model);
                return result;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> View(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cSecurityQuestion.ViewById(model.Id);
                return result;
            }
        );
    }
}
