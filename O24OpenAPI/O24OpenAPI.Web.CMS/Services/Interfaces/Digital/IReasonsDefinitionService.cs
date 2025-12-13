using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface IReasonsDefinitionService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<C_REASONS_DEFINITION> GetById(int id);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<C_REASONS_DEFINITION> Insert(C_REASONS_DEFINITION learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<ReasonsDefinitionViewModel>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<C_REASONS_DEFINITION> Update(C_REASONS_DEFINITION learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<C_REASONS_DEFINITION> DeleteById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<ReasonsDefinitionSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<ReasonsDefinitionSearchAdvanceResponseModel>> SearchAdvance(
        ReasonsDefinitionSearchAdvanceModel model
    );

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<ReasonsDefinitionViewModel> ViewById(int id);
}
