using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The SavingProductWorkflowService class
/// </summary>
/// <seealso cref="BaseQueueService"/>
public class SavingProductWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ISavingProductService _savingProductService =
        EngineContext.Current.Resolve<ISavingProductService>();

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
                var listData = await _savingProductService.GetAll();
                var response = new List<SavingProductViewResponseModel>();
                foreach (var item in listData)
                {
                    var entity = await _savingProductService.ViewById(item.Id, model.Language);
                    response.Add(entity);
                }
                return response;
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
                var result = await _savingProductService.ViewById(model.Id, model.Language);
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
        var model = await workflow.ToModel<SavingProductInsertModel>();
        return await Invoke<SavingProductInsertModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.ProductCode))
                {
                    throw new O24OpenAPIException("InvalidProductCode", "Product code not null");
                }

                var entity = model.FromModel<D_SAVING_PRODUCT>();
                entity.ProductName = JsonConvert.SerializeObject(model.ProductName);
                entity.Description = JsonConvert.SerializeObject(model.Description);
                var result = await _savingProductService.Insert(entity);
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
        var model = await workflow.ToModel<SavingProductUpdateModel>();
        return await Invoke<SavingProductUpdateModel>(
            workflow,
            async () =>
            {
                var entity = await _savingProductService.GetById(model.Id);
                if (entity == null)
                {
                    throw new O24OpenAPIException("InvalidProduct", "Product code does not exist");
                }

                entity = model.ToEntity(entity);
                entity.ProductName =
                    model.ProductName != null
                        ? JsonConvert.SerializeObject(model.ProductName)
                        : JsonConvert.SerializeObject(entity.ProductName);
                entity.Description =
                    model.Description != null
                        ? JsonConvert.SerializeObject(model.Description)
                        : JsonConvert.SerializeObject(entity.Description);
                var result = await _savingProductService.Update(entity);
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
                var entity = await _savingProductService.GetById(model.Id);
                if (entity == null)
                {
                    throw new O24OpenAPIException("InvalidProduct", "Product code does not exist");
                }

                var result = await _savingProductService.DeleteById(model.Id);
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
                var result = await _savingProductService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    SavingProductSimpleSearchResponseModel,
                    SavingProductSimpleSearchResponseModel
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
        var model = await workflow.ToModel<SavingProductAdvancedSearchRequestModel>();
        return await Invoke<SavingProductAdvancedSearchRequestModel>(
            workflow,
            async () =>
            {
                var result = await _savingProductService.AdvancedSearch(model);
                var items = result.ToPagedListModel<
                    SavingProductAdvancedSearchResponseModel,
                    SavingProductAdvancedSearchResponseModel
                >();
                return items;
            }
        );
    }
}
