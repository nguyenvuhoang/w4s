using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Sample.Services.Interfaces;

namespace O24OpenAPI.Sample.GrpcServices;

public class LoggerGrpcService
{
    private static readonly IServiceLogService _serviceLogService =
        EngineContext.Current.Resolve<IServiceLogService>();

    //[Web.Framework.Services.Grpc.GrpcInvocableMethod]
    //public async Task<string> WriteLog(string request)
    //{
    //    var log = JsonSerializer.Deserialize<ServiceLog>(request);
    //    await _serviceLogService.AddAsync(log);
    //    return "ok";
    //}
}
