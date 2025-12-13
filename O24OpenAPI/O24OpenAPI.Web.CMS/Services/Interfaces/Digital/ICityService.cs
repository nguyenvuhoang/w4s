using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ICityService
{

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="CityID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CITY> GetByCityID(int CityID);
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="CityName">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CITY> GetByCityName(string CityName);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_CITY>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="city">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CITY> Insert(D_CITY city);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="city">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CITY> Update(D_CITY city);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="CityID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CITY> DeleteById(int CityID);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_CITY>> Search(SearchCityModel model);
}
