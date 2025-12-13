using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.Logger.Services.Interfaces;

/// <summary>
/// The http log service interface
/// </summary>
public interface IHttpLogService
{
    /// <summary>
    /// Adds the log
    /// </summary>
    /// <param name="log">The log</param>
    Task AddAsync(HttpLog log);
}
