using O24OpenAPI.Client.Log;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.Domain.AggregateModels;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public partial interface ILogServiceService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;LogService&gt;.</returns>
    Task<LogService> GetById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="logService"></param>
    /// /// <returns></returns>
    LogServiceModel ToLogServiceModel(LogService logService);

    /// <summary>
    ///
    /// </summary>
    /// <param name="centralizedLog"></param>
    /// <returns></returns>
    Task WriteCentralizedLog(CentralizedLog centralizedLog);

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    Task<List<LogServiceModel>> GetAll();

    /// <summary>
    ///
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="logText"></param>
    /// <param name="details"></param>
    /// <param name="logType"></param>
    /// <returns></returns>
    Task WriteLog(string subject, string logText, string details = "{}", string logType = "Other");

    /// <summary>
    ///
    /// </summary>
    /// <param name="logService"></param>
    /// <returns></returns>
    Task Insert(LogService logService);

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    Task<LogService> Update(LogService orgPara);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    void DeleteAll();
}
