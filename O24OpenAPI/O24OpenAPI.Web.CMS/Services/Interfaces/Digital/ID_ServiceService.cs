using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ID_ServiceService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SERVICE> GetById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SERVICE> Insert(D_SERVICE learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<ServiceViewModel>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SERVICE> Update(D_SERVICE learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SERVICE> DeleteById(int id);

    /// <summary>
    ///
    /// </summary>
    /// /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<ServiceSearchSimpleResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<ServiceSearchAdvanceResponseModel>> SearchAdvance(
        ServiceSearchAdvanceModel model
    );

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<ServiceViewModel> ViewById(int id);
}
