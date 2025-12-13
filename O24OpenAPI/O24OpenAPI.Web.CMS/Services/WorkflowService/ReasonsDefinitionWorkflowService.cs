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

public class ReasonsDefinitionWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly IReasonsDefinitionService _cReasonsDefinition =
        EngineContext.Current.Resolve<IReasonsDefinitionService>();

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
                var listData = await _cReasonsDefinition.GetAll();
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
        var model = await workflow.ToModel<ReasonsDefinitionInsertModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cReasonsDefinition.Insert(
                    model.FromModel<C_REASONS_DEFINITION>()
                );
                var response = await _cReasonsDefinition.ViewById(result.Id);
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
        var model = await workflow.ToModel<ReasonsDefinitionUpdateModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cReasonsDefinition.Update(
                    model.FromModel<C_REASONS_DEFINITION>()
                );
                var response = await _cReasonsDefinition.ViewById(result.Id);
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
                var result = await _cReasonsDefinition.DeleteById(model.Id);
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
                var result = await _cReasonsDefinition.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    ReasonsDefinitionSearchSimpleResponseModel,
                    ReasonsDefinitionSearchSimpleResponseModel
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
        var model = await workflow.ToModel<ReasonsDefinitionSearchAdvanceModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cReasonsDefinition.SearchAdvance(model);
                var items = result.ToPagedListModel<
                    ReasonsDefinitionSearchAdvanceResponseModel,
                    ReasonsDefinitionSearchAdvanceResponseModel
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
    public async Task<WorkflowScheme> View(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<BaseTransactionModel>(
            workflow,
        async () =>
        {
                var result = await _cReasonsDefinition.ViewById(model.Id);
                return result;
            }
        );
    }
}
