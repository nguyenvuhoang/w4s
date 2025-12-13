using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The LoanProductWorkflowService class
/// </summary>
/// <seealso cref="BaseQueueService"/>
public class CardServiceWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ICardServiceService _cardServiceService =
        EngineContext.Current.Resolve<ICardServiceService>();

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
                var listEntity = await _cardServiceService.GetAll();
                return listEntity;
            }
        );
    }

    /// <summary>
    /// View
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
                var result = await _cardServiceService.ViewById(model.Id);
                return result;
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
        var model = await workflow.ToModel<CardServiceInsertModel>();
        return await Invoke<CardServiceInsertModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.CardServiceCode))
                {
                    throw new O24OpenAPIException(
                        "InvalidCardServiceCode",
                        "Card service code not null"
                    );
                }

                var entity = model.FromModel<D_CARD_SERVICE>();
                var result = await _cardServiceService.Insert(entity);
                return result;
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
        var model = await workflow.ToModel<CardServiceUpdateModel>();
        return await Invoke<CardServiceUpdateModel>(
            workflow,
            async () =>
            {
                var entity = await _cardServiceService.GetById(model.Id);
                if (entity == null)
                {
                    throw new O24OpenAPIException(
                        "InvalidCardServiceCode",
                        "Card service code does not exist"
                    );
                }

                entity = model.ToEntity(entity);
                var result = await _cardServiceService.Update(entity);
                return result;
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
                var result = await _cardServiceService.DeleteById(model.Id);
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
        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var result = await _cardServiceService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    CardServiceSearchResponseModel,
                    CardServiceSearchResponseModel
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
        var model = await workflow.ToModel<CardServiceAdvancedSearchRequestModel>();
        return await Invoke<CardServiceAdvancedSearchRequestModel>(
            workflow,
            async () =>
            {
                var result = await _cardServiceService.AdvancedSearch(model);
                var items = result.ToPagedListModel<
                    CardServiceSearchResponseModel,
                    CardServiceSearchResponseModel
                >();
                return items;
            }
        );
    }
}
