using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Models.Workflow;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Services;

/// <summary>
/// The workflow log service class
/// </summary>
/// <seealso cref="ILogService{WorkflowLog}"/>
public class WorkflowLogService(IRepository<WorkflowLog> repo) : ILogService<WorkflowLog>
{
    /// <summary>
    /// The repo
    /// </summary>
    private readonly IRepository<WorkflowLog> _repo = repo;

    /// <summary>
    /// Adds the log
    /// </summary>
    /// <param name="log">The log</param>
    public async Task AddAsync(WorkflowLog log)
    {
        await _repo.Insert(log);
    }

    /// <summary>
    /// GetByExecutionIdAsync
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<WorkflowLog> GetByExecutionIdAsync(string executionId)
    {
        var query = _repo.Table;
        var log = await query.FirstOrDefaultAsync(x => x.execution_id == executionId);
        return log;
    }

    /// <summary>
    /// Simples the search using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>The result</returns>
    public async Task<PagedModel> SimpleSearch(SearchModel model)
    {
        var query = _repo.Table;
        if (!string.IsNullOrWhiteSpace(model.SearchText))
        {
            query = query.Where(x =>
                x.workflow_id.Contains(model.SearchText) ||
                x.execution_id.Contains(model.SearchText)
            );
        }
        query = query.Where(x => !x.workflow_id.Contains("LOG_SIMPLE_SEARCH"));
        query = query.OrderByDescending(x => x.Id);
        var pageList = await query.ToPagedList(model.PageIndex, model.PageSize);
        var result = pageList.ToPagedListModel<WorkflowLog, WorkflowLogSearchResponse>();
        return result;
    }
}
