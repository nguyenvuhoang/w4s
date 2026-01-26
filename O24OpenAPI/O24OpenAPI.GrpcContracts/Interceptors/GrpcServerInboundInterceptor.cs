using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using LinKit.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Logging.Enums;
using O24OpenAPI.Logging.Helpers;
using Serilog;
using Serilog.Context;

namespace O24OpenAPI.GrpcContracts.Interceptors;

[RegisterService(Lifetime.Singleton, serviceType: typeof(GrpcServerInboundInterceptor))]
public class GrpcServerInboundInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Dictionary<string, string> headers = context
            .RequestHeaders.Where(e => !e.IsBinary)
            .ToDictionary(e => e.Key, e => e.Value);

        HttpContext httpContext = context.GetHttpContext();
        string pseudoUrl =
            $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";

        using AsyncServiceScope scope = httpContext.RequestServices.CreateAsyncScope();
        AsyncScope.Scope = scope;

        TResponse? response = default;
        Exception? exceptionToLog = null;

        try
        {
            // 1. Set WorkContext
            string? stringWorkContext =
                headers.GetValueOrDefault("work_context")
                ?? headers.GetValueOrDefault("WORK_CONTEXT");
            if (!string.IsNullOrWhiteSpace(stringWorkContext))
            {
                WorkContextTemplate? workContextTemplate =
                    stringWorkContext.ToObject<WorkContextTemplate>();
                if (workContextTemplate != null)
                    EngineContext
                        .Current.Resolve<WorkContext>()
                        ?.SetWorkContext(workContextTemplate);
            }

            string? correlationId = EngineContext.Current.Resolve<WorkContext>()?.ExecutionLogId;

            using (LogContext.PushProperty(LogContextHelper.CorrelationIdKey, correlationId))
            {
                try
                {
                    response = await continuation(request, context);
                    return response;
                }
                catch (Exception ex)
                {
                    exceptionToLog = ex;

                    if (typeof(TResponse) == typeof(GrpcResponse))
                    {
                        response = (TResponse)(object)HandleException(ex);
                        return response;
                    }

                    throw;
                }
            }
        }
        finally
        {
            stopwatch.Stop();
            LogGrpcCall(
                pseudoUrl,
                request,
                response,
                exceptionToLog,
                stopwatch.ElapsedMilliseconds,
                headers
            );
        }
    }

    private static GrpcResponse HandleException(Exception ex)
    {
        if (ex is O24OpenAPIException o24Ex)
        {
            return new GrpcResponse
            {
                Code = GrpcResponseCode.Fail,
                Message = o24Ex.Message,
                ErrorCode = o24Ex.ErrorCode ?? string.Empty,
                Detail = o24Ex.StackTrace,
            };
        }

        return new GrpcResponse
        {
            Code = GrpcResponseCode.Fail,
            Message = ex.Message,
            ErrorCode = "ERR_INTERNAL",
            Detail = ex.StackTrace,
        };
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
        ILogger logger = Log.ForContext("LogType", LogType.Grpc)
            .ForContext("Direction", LogDirection.In)
            .ForContext("Action", method)
            .ForContext("Request", request.WriteIndentedJson())
            .ForContext("Response", response.WriteIndentedJson())
            .ForContext("Error", exception?.ToString())
            .ForContext("Duration", duration)
            .ForContext("Headers", headers.WriteIndentedJson())
            .ForContext("Flow", headers.GetValueOrDefault("flow"));
        if (exception is not null)
        {
            logger.Error(exception, exception.Message);
        }
        else
        {
            logger.Information("gRPC Call {Method} executed in {Duration}ms", method, duration);
        }
    }
}
