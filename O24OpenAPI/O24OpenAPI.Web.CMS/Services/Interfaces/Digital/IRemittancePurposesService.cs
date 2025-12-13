using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IRemittancePurposesService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REMITTANCE_PURPOSES> GetById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REMITTANCE_PURPOSES> Insert(D_REMITTANCE_PURPOSES learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<RemittancePurposesViewModel>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REMITTANCE_PURPOSES> Update(D_REMITTANCE_PURPOSES learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_REMITTANCE_PURPOSES> DeleteById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<RemittancePurposesSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<RemittancePurposesSearchAdvanceResponseModel>> SearchAdvance(
        RemittancePurposesSearchAdvanceModel model
    );

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<RemittancePurposesViewModel> ViewById(int id);
}
