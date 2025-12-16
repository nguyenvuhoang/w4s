using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The LoanProductWorkflowService class
/// </summary>
/// <seealso cref="BaseQueueService"/>
public class CardUserWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ICardUserService _cardUserService =
        EngineContext.Current.Resolve<ICardUserService>();

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var listEntity = await _cardUserService.GetAll(model.Language);
                return listEntity;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> GetListByUserCode(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithUserCode>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cardUserService.GetListCardByUserCode(
                    model.UserCode,
                    model.Language
                );
                return result;
            }
        );
    }

    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> GetByUserCode(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithUserCode>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _cardUserService.GetCardByUserCode(model.UserCode);
                return result;
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
                var result = await _cardUserService.ViewById(model.Id, model.Language);
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
        var model = await workflow.ToModel<CardUserInsertModel>();
        return await Invoke<CardUserInsertModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.CardCode))
                {
                    throw new O24OpenAPIException("InvalidCardCode", "Card code not null");
                }

                var entity = model.FromModel<D_CARD_USER>();
                var result = await _cardUserService.Insert(entity);
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
        var model = await workflow.ToModel<CardUserUpdateModel>();
        return await Invoke<CardUserUpdateModel>(
            workflow,
            async () =>
            {
                var entity = await _cardUserService.GetById(model.Id);
                if (entity == null)
                {
                    throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
                }

                entity = model.ToEntity(entity);
                var result = await _cardUserService.Update(entity);
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
                var result = await _cardUserService.DeleteById(model.Id);
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
                var result = await _cardUserService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    CardUserSearchResponseModel,
                    CardUserSearchResponseModel
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
        var model = await workflow.ToModel<CardUserAdvancedSearchRequestModel>();
        return await Invoke<CardUserAdvancedSearchRequestModel>(
            workflow,
            async () =>
            {
                var result = await _cardUserService.AdvancedSearch(model);
                var items = result.ToPagedListModel<
                    CardUserSearchResponseModel,
                    CardUserSearchResponseModel
                >();
                return items;
            }
        );
    }
}
