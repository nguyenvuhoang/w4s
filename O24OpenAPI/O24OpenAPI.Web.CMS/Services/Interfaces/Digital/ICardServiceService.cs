using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The loan product interface
/// </summary>

public interface ICardServiceService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<D_CARD_SERVICE> GetById(int id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<CardServiceViewResponseModel>> GetAll();

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="code">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<CardServiceViewResponseModel> GetByCardServiceCode(string code);

    /// <summary>
    /// ViewById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CardServiceViewResponseModel> ViewById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="card">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_SERVICE> Insert(D_CARD_SERVICE card);

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="product">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_SERVICE> Update(D_CARD_SERVICE card);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="productId">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_SERVICE> DeleteById(int cardId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardServiceSearchResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardServiceSearchResponseModel>> AdvancedSearch(
        CardServiceAdvancedSearchRequestModel model
    );
}
