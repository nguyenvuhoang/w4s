using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The loan product interface
/// </summary>

public interface ICardUserService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<D_CARD_USER> GetById(int id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<CardUserViewResponseModel>> GetAll(string lang);

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="code">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<CardUserViewResponseModel> GetCardByUserCode(string code);

    /// <summary>
    /// Gets the list by user code using the specified code
    /// </summary>
    /// <param name="code">The code</param>
    /// <param name="lang">The language</param>
    /// <returns>A task containing the list</returns>
    Task<List<CardUserViewResponseModel>> GetListCardByUserCode(string code, string lang);

    /// <summary>
    /// ViewById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CardUserViewResponseModel> ViewById(int id, string lang);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="card">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_USER> Insert(D_CARD_USER card);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="product">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_USER> Update(D_CARD_USER card);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="cardId">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD_USER> DeleteById(int cardId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardUserSearchResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardUserSearchResponseModel>> AdvancedSearch(CardUserAdvancedSearchRequestModel model);
}
