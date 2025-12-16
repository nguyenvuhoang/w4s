using MediatR;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logger.Models.Log;
using O24OpenAPI.Logger.Services.QueryHandler;
using O24OpenAPI.Logger.Utils;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Logger.Controllers;

/// <summary>
/// The log controller class
/// </summary>
/// <seealso cref="BaseController"/>
public class LogController : BaseController
{
    /// <summary>
    /// Tests the log using the specified message
    /// </summary>
    /// <param name="logType"></param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public async Task<IActionResult> TestSearchLog(string logType)
    {
        var searchModel = new SearchModel() { LogType = logType };

        // var service = EngineContext.Current.Resolve<ILogService<ServiceLog>>();
        var service = EngineContext.Current.Resolve<IMediator>();
        // var r = await service.SimpleSearch(searchModel);
        var type = TypeLogUtils.GetTypeLog(logType);
        var queryType = typeof(SimpleSearchQuery<>).MakeGenericType(type);
        var queryModel = Activator.CreateInstance(queryType, searchModel);
        var r = await service.Send((IRequest<PagedModel>)queryModel);
        return Ok(r);
    }

}
