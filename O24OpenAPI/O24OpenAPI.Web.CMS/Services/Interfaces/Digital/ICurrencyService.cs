using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ICurrencyService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="currencycode">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CURRENCY> GetByCurrencyCode(string currencycode);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_CURRENCY>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="currency">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CURRENCY> Insert(D_CURRENCY currency);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="currency">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_CURRENCY> Update(D_CURRENCY currency);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="CurrencyCode">The id</param>
    /// <returns>A task containing the bank</returns>
    Task DeleteById(DeleteCurrencyByCurrencyCodeModel model);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_CURRENCY>> Search(SearchCurrencyModel model);
}
