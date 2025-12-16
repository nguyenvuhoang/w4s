using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class ProductWorkflowService(IProductService productService) : BaseQueueService
{
    private readonly IProductService _productService = productService;

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _productService.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ProductModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _productService.Insert(model.FromModel<D_PRODUCT>());
                var response = await _productService.ViewById(result.Id);
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ProductModel>();

        if (string.IsNullOrEmpty(model.ProductID))
        {
            throw new O24OpenAPIException("InvalidProductID", "The Product ID is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var product = await _productService.GetByProductID(model.ProductID);
                string usercreated = product.UserCreated;
                DateTime datecreated = (DateTime)product.DateCreated;

                var updateproduct = model.ToEntity(product);
                if (string.IsNullOrEmpty(updateproduct.UserCreated))
                {
                    updateproduct.UserCreated = usercreated;
                }

                if (updateproduct.DateCreated == null)
                {
                    updateproduct.DateCreated = datecreated;
                }

                await _productService.Update(updateproduct);
                return updateproduct;
            }
        );
    }

    public async Task<WorkflowScheme> Delete(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ProductModel>();

        if (string.IsNullOrEmpty(model.ProductID))
        {
            throw new O24OpenAPIException("InvalidProductID", "The Product ID is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var product = await _productService.DeleteByProductId(model.ProductID);
                return product;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteByListID(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<DeleteProductModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                await _productService.DeleteByListID(model);
                return model;
            }
        );
    }
}
