namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IParaServerService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    Task<ParaServer> GetById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<List<ParaServer>> GetAll();

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<ParaServer> GetByAppAndCode(string app, string code);

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    Task<List<ParaServer>> GetByApp(string app);

    /// <summary>
    ///
    /// </summary>
    /// <param name="ParaServer"></param>
    /// <returns></returns>
    Task Insert(ParaServer ParaServer);

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    Task Update(ParaServer ParaServer);

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    Task<ParaServer> DeleteByAppAndParaServer(string app, string ParaServer);
}
