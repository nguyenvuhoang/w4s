using Grpc.Core;
using LinKit.Json.Runtime;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.LOG;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Services.Interfaces;
using static O24OpenAPI.Grpc.LOG.LOGGrpcService;

namespace O24OpenAPI.Logger.GrpcServices;

/// <summary>
///
/// </summary>
public class LOGGrpcService : LOGGrpcServiceBase
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GrpcResponse> Test(TestRequest request, ServerCallContext context)
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                // Simulate some processing
                await Task.Delay(100);
                return new GrpcResponse
                {
                    Code = GrpcResponseCode.Success,
                    Message = "Test successful",
                    Data = $"Hello {request.Message}",
                };
            }
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GrpcResponse> SubmitLog(
        GrpcRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var data = request.Data;
                var applicationLogService = EngineContext.Current.Resolve<IApplicationLogService>();
                var applicationLog = data.FromJson<ApplicationLog>();
                await applicationLogService.AddAsync(applicationLog);
                return new GrpcResponse { Code = GrpcResponseCode.Success, Data = "ok" };
            }
        );
    }
}
