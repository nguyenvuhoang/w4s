using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ICountryService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_COUNTRY> GetById(int id);
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_COUNTRY> Insert(D_COUNTRY learnApi);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<CountryViewModel>> GetAll();
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_COUNTRY> Update(D_COUNTRY learnApi);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_COUNTRY> DeleteById(int id);
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SearchSimpleResponseModel>> SimpleSearch(SimpleSearchModel model);
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SearchAdvanceResponseModel>> SearchAdvance(CountrySearchAdvanceModel model);
    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<CountryViewModel> ViewById(int id);

    Task DeleteByListID(DeleteCountryModel model);

    Task<JToken> GetCountryName(string countryCode);

    Task<JToken> GetCountryList();
}
