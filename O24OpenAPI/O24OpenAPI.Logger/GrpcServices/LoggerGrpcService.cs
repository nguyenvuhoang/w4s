using System.Text.Json;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services.Grpc;

namespace O24OpenAPI.Logger.GrpcServices;

/// <summary>
/// The logger grpc service class
/// </summary>
public class LoggerGrpcService
{
    /// <summary>
    /// The service log service
    /// </summary>
    private static readonly IServiceLogService _serviceLogService =
        EngineContext.Current.Resolve<IServiceLogService>();

    /// <summary>
    /// Writes the log using the specified request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the string</returns>
    [GrpcInvocableMethod]
    public async Task<string> WriteLog(string request)
    {
        var log = JsonSerializer.Deserialize<ServiceLog>(request);
        await _serviceLogService.AddAsync(log);
        return "ok";
    }
}
