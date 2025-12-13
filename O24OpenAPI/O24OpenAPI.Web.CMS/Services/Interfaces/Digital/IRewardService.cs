using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IRewardService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REWARDS> GetByRewardID(int Id);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_REWARDS>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="reward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REWARDS> Insert(D_REWARDS reward);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="reward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REWARDS> Update(D_REWARDS reward);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REWARDS> DeleteById(int Id);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_REWARDS>> Search(SearchRewardModel model);
}
