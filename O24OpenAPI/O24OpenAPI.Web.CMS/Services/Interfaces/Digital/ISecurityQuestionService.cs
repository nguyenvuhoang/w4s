using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ISecurityQuestionService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SECURITY_QUESTION> GetById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SECURITY_QUESTION> Insert(D_SECURITY_QUESTION learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<SecurityQuestionViewModel>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SECURITY_QUESTION> Update(D_SECURITY_QUESTION learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_SECURITY_QUESTION> DeleteById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SecurityQuestionSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<SecurityQuestionSearchAdvanceResponseModel>> SearchAdvance(
        SecurityQuestionSearchAdvanceModel model
    );

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<SecurityQuestionViewModel> ViewById(int id);
}
