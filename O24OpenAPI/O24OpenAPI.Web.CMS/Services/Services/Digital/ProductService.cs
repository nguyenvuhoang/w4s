using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class ProductService : IProductService
{
    private readonly IRepository<D_PRODUCT> _productRepository;

    public ProductService(IRepository<D_PRODUCT> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task DeleteByListID(DeleteProductModel model)
    {
        model.ListProductID.Add(model.ProductID);
        foreach (string proID in model.ListProductID)
        {
            var product = await _productRepository
            .Table.Where(s => s.ProductID.Equals(proID))
            .FirstOrDefaultAsync();

            if (product != null)
            {
                await _productRepository.Delete(product);
            }

        }

    }

    public virtual async Task<D_PRODUCT> DeleteByProductId(string productID)
    {
        var product = await _productRepository
            .Table.Where(s => s.ProductID.Equals(productID))
            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new O24OpenAPIException(
                "InvalidProduct",
                "The Prooduct ID does not exist in system"
            );
        }
        await _productRepository.Delete(product);
        return product;
    }

    public virtual async Task<List<D_PRODUCT>> GetAll()
    {
        return await _productRepository.Table.Select(s => s).ToListAsync();
    }

    public virtual async Task<D_PRODUCT> GetByProductID(string productID)
    {
        return await _productRepository
            .Table.Where(s => s.ProductID == productID)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<D_PRODUCT> Insert(D_PRODUCT product)
    {
        var findProduct = await _productRepository
            .Table.Where(s => s.ProductID.Equals(product.ProductID))
            .FirstOrDefaultAsync();
        if (findProduct == null)
        {
            await _productRepository.Insert(product);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidProduct",
                "The Product ID already existing in system"
            );
        }

        return product;
    }

    public virtual async Task<D_PRODUCT> Update(D_PRODUCT product)
    {
        await _productRepository.Update(product);
        return product;
    }

    public virtual async Task<ProductModel> ViewById(int id)
    {
        try
        {
            var entity = await _productRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Product does not exist");
            }

            var result = entity.ToModel<ProductModel>();
            return result;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }
}
