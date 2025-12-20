using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// /// The LoanProductWorkflowService class
/// </summary>
/// <seealso cref="BaseQueueService"/>
public class CardWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ICardService _cardService = EngineContext.Current.Resolve<ICardService>();

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
                var listEntity = await _cardService.GetCardInformation();
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
                var result = await _cardService.ViewById(model.Id);
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
        var model = await workflow.ToModel<CardInsertModel>();
        return await Invoke<CardInsertModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.CardCode))
                {
                    throw new O24OpenAPIException("InvalidCardCode", "Card code not null");
                }

                var entity = model.FromModel<D_CARD>();
                var result = await _cardService.Insert(entity);
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
        var model = await workflow.ToModel<CardUpdateModel>();
        return await Invoke<CardUpdateModel>(
            workflow,
            async () =>
            {
                var entity = await _cardService.GetById(model.Id);
                if (entity == null)
                {
                    throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
                }

                entity = model.ToEntity(entity);
                var result = await _cardService.Update(entity);
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
                var result = await _cardService.DeleteById(model.Id);
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
                var result = await _cardService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    CardSearchResponseModel,
                    CardSearchResponseModel
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
        var model = await workflow.ToModel<CardAdvancedSearchRequestModel>();
        return await Invoke<CardAdvancedSearchRequestModel>(
            workflow,
            async () =>
            {
                var result = await _cardService.AdvancedSearch(model);
                var items = result.ToPagedListModel<
                    CardSearchResponseModel,
                    CardSearchResponseModel
                >();
                return items;
            }
        );
    }
}
