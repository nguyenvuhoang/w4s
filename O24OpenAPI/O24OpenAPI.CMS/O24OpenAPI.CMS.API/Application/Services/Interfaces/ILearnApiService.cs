using O24OpenAPI.CMS.API.Application.Models;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public partial interface ILearnApiService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    Task<LearnApi> GetById(int id);

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    Task<LearnApiModel> GetByAppAndId(string app, string learnApiId);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<PagedListModel<LearnApi, LearnApiModel>> GetAll(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    Task<List<LearnApiModel>> GetByApp(string app);

    /// <summary>
    ///
    /// </summary>
    /// <param name="LearnApi"></param>
    /// <returns></returns>
    Task<LearnApi> Insert(LearnApi LearnApi);

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    Task<LearnApi> Update(LearnApi LearnApi);

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    Task<LearnApi> DeleteById(int id);

    Task<LearnApi> GetByLearnApiIdAndChannel(string learnApiId, string channelId);
}
