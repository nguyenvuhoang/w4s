using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Models.ServiceLog;
using O24OpenAPI.Logger.Services.Interfaces;

namespace O24OpenAPI.Logger.Services;

/// <summary>
/// The service log service class
/// </summary>
/// <seealso cref="ILogService{ServiceLog}"/>
public class ServiceLogService(IRepository<ServiceLog> serviceLogRepository)
    : ILogService<ServiceLog>
{
    /// <summary>
    /// The service log repository
    /// </summary>
    private readonly IRepository<ServiceLog> _serviceLogRepository = serviceLogRepository;

    /// <summary>
    /// Adds the channel log
    /// </summary>
    /// <param name="channelLog">The channel log</param>
    public async Task AddAsync(ServiceLog channelLog)
    {
        await _serviceLogRepository.Insert(channelLog);
    }

    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<ServiceLog> GetByExecutionIdAsync(string executionId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Simples the search using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the paged list model</returns>
    public async Task<PagedModel> SimpleSearch(SearchModel model)
    {
        var query =
            from d in _serviceLogRepository.Table
            where
                !string.IsNullOrEmpty(d.ChannelId)
                    && !string.IsNullOrEmpty(model.ChannelId)
                    && d.ChannelId == model.ChannelId
                || true
            select d;
        var pageList = await query.ToPagedList(model.PageIndex, model.PageSize);
        var result = pageList.ToPagedListModel<ServiceLog, ServiceLogSearchResponse>();
        return result;
        // return new JObject { { "hello", "linh nè" } };
    }
}
