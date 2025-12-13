using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IRequestRewardService
{
    /// <summary>
    /// /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REQUESTREWARD> GetByRequestRewardID(int Id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_REQUESTREWARD>> GetAll();

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="requestreward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REQUESTREWARD> Insert(D_REQUESTREWARD requestreward);

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="requestreward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REQUESTREWARD> Update(D_REQUESTREWARD requestreward);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REQUESTREWARD> DeleteById(int Id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_REQUESTREWARD>> Search(SearchRequestRewardModel model);
}
