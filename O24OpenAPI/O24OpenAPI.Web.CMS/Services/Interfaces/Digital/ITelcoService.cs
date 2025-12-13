using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ITelcoService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="TelcoName">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_TOP_TELCO> GetByTelcoName(string TelcoName);
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="TelcoID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_TOP_TELCO> GetByTelcoID(int TelcoID);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_TOP_TELCO>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="telco">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_TOP_TELCO> Insert(D_TOP_TELCO telco);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="telco">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_TOP_TELCO> Update(D_TOP_TELCO telco);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="TelcoID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_TOP_TELCO> DeleteById(int TelcoID);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_TOP_TELCO>> Search(SearchTelcoModel model);
}
