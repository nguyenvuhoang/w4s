using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logger.Models.HttpLog;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Services.Interfaces;

namespace O24OpenAPI.Logger.Services;

/// <summary>
/// The http log service class
/// </summary>
/// <seealso cref="ILogService{HttpLog}"/>
public class HttpLogService(IRepository<HttpLog> repository) : ILogService<HttpLog>
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IRepository<HttpLog> _repository = repository;

    /// <summary>
    /// Adds the log
    /// </summary>
    /// <param name="log">The log</param>
    public async Task AddAsync(HttpLog log)
    {
        await _repository.Insert(log);
    }

    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<HttpLog> GetByExecutionIdAsync(string executionId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Simples the search using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>A task containing the object</returns>
    public async Task<PagedModel> SimpleSearch(SearchModel model)
    {
        var query = _repository.Table;
        var pageList = await query.ToPagedList(model.PageIndex, model.PageSize);
        var result = pageList.ToPagedListModel<HttpLog, HttpLogSearchResponse>();
        return result;
    }
}
