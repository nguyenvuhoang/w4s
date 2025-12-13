using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IUserRewardService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_USER_REWARD> GetByUserRewardID(int Id);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_USER_REWARD>> GetAll();
    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="userreward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_USER_REWARD> Insert(D_USER_REWARD userreward);
    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="reward">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_USER_REWARD> Update(D_USER_REWARD userreward);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_USER_REWARD> DeleteById(int Id);
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_USER_REWARD>> Search(SearchUserRewardModel model);
    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="usercode">usercode</param>
    /// <returns>A task containing the bank</returns>
    Task<decimal> GetTotalPoint(string usercode);
}
