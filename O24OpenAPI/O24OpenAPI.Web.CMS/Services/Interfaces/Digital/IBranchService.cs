using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IBranchService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// /// </summary>
    /// <param name="BranchID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BRANCH> GetByBranchCode(string BranchID);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_BRANCH>> GetAll();

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="branch">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BRANCH> Insert(D_BRANCH branch);

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="branch">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BRANCH> Update(D_BRANCH branch);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="BranchID">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BRANCH> DeleteById(string BranchID);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_BRANCH>> Search(SearchBranchModel model);

    Task DeleteByListID(DeleteBranchByBranchCodeModel model);
}
