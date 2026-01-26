using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Grpc;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;
using System.Text.Json;

namespace O24OpenAPI.Logger.API.GrpcServices;

/// <summary>
/// The logger grpc service class
/// </summary>
public class LoggerGrpcService
{
    /// <summary>
    /// The service log service
    /// </summary>
    private static readonly IServiceLogRepository serviceLogRepository =
        EngineContext.Current.Resolve<IServiceLogRepository>();

    /// <summary>
    /// Writes the log using the specified request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the string</returns>
    [GrpcInvocableMethod]
    public async Task<string> WriteLog(string request)
    {
        ServiceLog log = JsonSerializer.Deserialize<ServiceLog>(request);
        await serviceLogRepository.InsertAsync(log);
        return "ok";
    }
}
