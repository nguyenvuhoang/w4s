using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The loan product interface
/// </summary>

public interface ISavingProductService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<D_SAVING_PRODUCT> GetById(int id);
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="productCode">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SAVING_PRODUCT> GetByProductCode(string productCode);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_SAVING_PRODUCT>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="product">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SAVING_PRODUCT> Insert(D_SAVING_PRODUCT product);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="product">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SAVING_PRODUCT> Update(D_SAVING_PRODUCT product);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="productId">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SAVING_PRODUCT> DeleteById(int productId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SavingProductSimpleSearchResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SavingProductAdvancedSearchResponseModel>> AdvancedSearch(SavingProductAdvancedSearchRequestModel model);

    /// <summary>
    /// View by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SavingProductViewResponseModel> ViewById(int id, string lang);
}
