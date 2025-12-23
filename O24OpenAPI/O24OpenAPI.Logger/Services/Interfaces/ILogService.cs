using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logger.Models.Log;

namespace O24OpenAPI.Logger.Services.Interfaces;

/// <summary>
/// The log service interface
/// </summary>
public interface ILogService<T>
    where T : class
{
    /// <summary>
    /// Adds the log
    /// </summary>
    /// <param name="log">The log</param>
    Task AddAsync(T log);

    /// <summary>
    /// Simples the search using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the object</returns>
    Task<PagedModel> SimpleSearch(SearchModel model);

    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    Task<T> GetByExecutionIdAsync(string executionId);
}
