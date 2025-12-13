using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Services.Interfaces;

/// <summary>
/// The service log service interface
/// </summary>
public interface IServiceLogService
{
    /// <summary>
    /// Adds the channel log
    /// </summary>
    /// <param name="channelLog">The channel log</param>
    Task AddAsync(ServiceLog channelLog);
}
