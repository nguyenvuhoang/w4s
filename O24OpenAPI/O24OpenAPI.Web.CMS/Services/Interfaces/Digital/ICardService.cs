using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The loan product interface
/// </summary>

public interface ICardService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<D_CARD> GetById(int id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<CardViewResponseModel>> GetAll();

    /// <summary>
    /// GetCardInformation
    /// </summary>
    /// <returns></returns>
    Task<List<GetCardInformationModel>> GetCardInformation();

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="code">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<CardViewResponseModel> GetByCardCode(string code);

    /// <summary>
    /// ViewById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CardViewResponseModel> ViewById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="card">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD> Insert(D_CARD card);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="product">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD> Update(D_CARD card);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="productId">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CARD> DeleteById(int cardId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardSearchResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CardSearchResponseModel>> AdvancedSearch(CardAdvancedSearchRequestModel model);
}
