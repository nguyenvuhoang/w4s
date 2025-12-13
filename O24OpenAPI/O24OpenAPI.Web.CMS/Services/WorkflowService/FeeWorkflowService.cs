using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The bankworkflowservice class
/// </summary>
/// <seealso cref="BaseQueueService"/>

public class FeeWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly IFeeService _cFeeWorkflowService =
        EngineContext.Current.Resolve<IFeeService>();

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
                var listData = await _cFeeWorkflowService.GetAll();
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
        var model = await workflow.ToModel<FeeInsertModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cFeeWorkflowService.Insert(model.FromModel<D_FEE>());
                var response = await _cFeeWorkflowService.ViewById(result.Id);
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
        var model = await workflow.ToModel<FeeUpdateModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cFeeWorkflowService.Update(model.FromModel<D_FEE>());
                var response = await _cFeeWorkflowService.ViewById(result.Id);
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
                var result = await _cFeeWorkflowService.DeleteById(model.Id);
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
                var result = await _cFeeWorkflowService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    FeeSearchSimpleResponseModel,
                    FeeSearchSimpleResponseModel
                >();
                return items;
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
        var model = await workflow.ToModel<FeeSearchAdvanceModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                try
                {
                    var result = await _cFeeWorkflowService.SearchAdvance(model);
                    var items = result.ToPagedListModel<
                        FeeSearchAdvanceResponseModel,
                        FeeSearchAdvanceResponseModel
                    >();
                    return items;
                }
                catch (Exception ex)
                {
                    throw new O24OpenAPIException(ex.Message, ex);
                }
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
                var result = await _cFeeWorkflowService.ViewById(model.Id);
                return result;
            }
        );
    }
}
