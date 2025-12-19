using O24OpenAPI.CMS.API.Application.Models;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

/// <summary>
/// User Account service
/// </summary>
public partial interface IFormService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task<Form> GetById(int id);

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task<FormModel> GetByIdAndApp(string formCode, string app);

    /// <summary>
    /// Gets SearchByApp
    /// </summary>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task<List<FormModel>> GetByApp(string app);

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    Task<List<RoleCacheModel>> GetRoleCacheByApp(string app);

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task Insert(Form form);

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task Update(Form form);

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Form&gt;.</returns>
    Task Delete(string tx_code, string app);
    Task FeedDataRequestMapping();
    Task<List<Form>> GetAll();
}
