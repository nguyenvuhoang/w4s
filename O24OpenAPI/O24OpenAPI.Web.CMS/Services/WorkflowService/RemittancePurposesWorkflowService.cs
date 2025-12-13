using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The bankworkflowservice class
/// </summary>
/// <seealso cref="BaseQueueService"/>

public class RemittancePurposesWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly IRemittancePurposesService _dremittancePurposes =
        EngineContext.Current.Resolve<IRemittancePurposesService>();

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
                var listData = await _dremittancePurposes.GetAll();
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
        var model = await workflow.ToModel<RemittancePurposesInsertModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _dremittancePurposes.Insert(
                    model.FromModel<D_REMITTANCE_PURPOSES>()
                );
                var response = await _dremittancePurposes.ViewById(result.Id);
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
        var model = await workflow.ToModel<RemittancePurposesUpdateModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _dremittancePurposes.Update(
                    model.FromModel<D_REMITTANCE_PURPOSES>()
                );
                var response = await _dremittancePurposes.ViewById(result.Id);
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
                var result = await _dremittancePurposes.DeleteById(model.Id);
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
                var result = await _dremittancePurposes.SimpleSearch(model);
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
        var model = await workflow.ToModel<RemittancePurposesSearchAdvanceModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _dremittancePurposes.SearchAdvance(model);
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
                var result = await _dremittancePurposes.ViewById(model.Id);
                return result;
            }
        );
    }
}
