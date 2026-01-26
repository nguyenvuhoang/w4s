using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Enums;
using O24OpenAPI.Logging.Helpers;
using Serilog;

namespace O24OpenAPI.Logging.Interceptors;

public class GrpcLoggingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        var correlationId = EngineContext.Current.ResolveRequired<WorkContext>().ExecutionLogId;
        var headers = context
            .RequestHeaders.Where(e => !e.IsBinary)
            .ToDictionary(e => e.Key, e => e.Value);

        using (
            Serilog.Context.LogContext.PushProperty(
                LogContextHelper.CorrelationIdKey,
                correlationId
            )
        )
        {
            var stopwatch = Stopwatch.StartNew();
            Exception? exception = null;
            TResponse? response = default;

            try
            {
                response = await base.UnaryServerHandler(request, context, continuation);
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                LogGrpcCall(
                    context.Method,
                    request,
                    response,
                    exception,
                    stopwatch.ElapsedMilliseconds,
                    headers
                );
            }
        }
    }

    private static void LogGrpcCall<TRequest, TResponse>(
        string method,
        TRequest request,
        TResponse? response,
        Exception? exception,
        long duration,
        Dictionary<string, string> headers
    )
    {
        var prettyRequest = request.WriteIndentedJson();
        var prettyResponse = response.WriteIndentedJson();

        Log.ForContext("LogType", LogType.Grpc)
            .ForContext("Direction", LogDirection.In)
            .ForContext("Action", method)
            .ForContext("Request", prettyRequest)
            .ForContext("Response", prettyResponse)
            .ForContext("Error", exception)
            .ForContext("Duration", duration)
            .ForContext("Headers", headers.WriteIndentedJson())
            .ForContext(
                "Flow",
                headers.TryGetValue("flow", out var flowValue) ? flowValue.ToString() : null
            )
            .Information("gRPC Call Log");
    }
}
