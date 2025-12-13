using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IFeeTypeService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_FEE_TYPE> GetById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_FEE_TYPE> Insert(D_FEE_TYPE learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<FeeTypeViewModel>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_FEE_TYPE> Update(D_FEE_TYPE learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_FEE_TYPE> DeleteById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<FeeTypeSearchSimpleResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<FeeTypeSearchAdvanceResponseModel>> SearchAdvance(
        FeeTypeSearchAdvanceModel model
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<FeeTypeViewModel> ViewById(int id);
}
