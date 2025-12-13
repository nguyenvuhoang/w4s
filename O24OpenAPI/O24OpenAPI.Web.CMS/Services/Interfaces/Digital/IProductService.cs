using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface IProductService
{
    /// <summary>
    /// Get all product
    /// </summary>
    /// <returns></returns>
    Task<List<D_PRODUCT>> GetAll();

    /// <summary>
    /// Insert product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<D_PRODUCT> Insert(D_PRODUCT product);
    Task<D_PRODUCT> GetByProductID(string productID);

    Task<ProductModel> ViewById(int id);

    Task<D_PRODUCT> Update(D_PRODUCT product);
    Task<D_PRODUCT> DeleteByProductId(string productID);
    Task DeleteByListID(DeleteProductModel model);
}
