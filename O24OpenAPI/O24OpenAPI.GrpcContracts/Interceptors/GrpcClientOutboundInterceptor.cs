using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using LinKit.Core.Abstractions;
using LinKit.Json.Runtime;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Enums;
using O24OpenAPI.Logging.Helpers;
using Serilog;

namespace O24OpenAPI.GrpcContracts.Interceptors;

[RegisterService(Lifetime.Transient, serviceType: typeof(GrpcClientOutboundInterceptor))]
public sealed class GrpcClientOutboundInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation
    )
    {
        string pseudoUrl = $"grpc://{context.Host}{context.Method.FullName}";

        WorkContext? workContext = EngineContext.Current.Resolve<WorkContext>();
        string correlationId = workContext?.ExecutionLogId ?? Guid.NewGuid().ToString();

        Metadata headers = context.Options.Headers ?? [];
        headers.Add("x-correlation-id", correlationId);
        headers.Add("work_context", workContext.ToJson() ?? "{}");

        CallOptions newOptions = context.Options.WithHeaders(headers);
        ClientInterceptorContext<TRequest, TResponse> newContext = new(
            context.Method,
            context.Host,
            newOptions
        );

        Stopwatch stopwatch = Stopwatch.StartNew();

        AsyncUnaryCall<TResponse> call = continuation(request, newContext);

        async Task<TResponse> HandleResponseAsync()
        {
            using (
                Serilog.Context.LogContext.PushProperty(
                    LogContextHelper.CorrelationIdKey,
                    correlationId
                )
            )
            {
                try
                {
                    TResponse response = await call.ResponseAsync.ConfigureAwait(false);

                    LogOutgoingGrpcCall(
                        pseudoUrl,
                        request,
                        response,
                        null,
                        stopwatch.ElapsedMilliseconds,
                        headers
                    );

                    return response;
                }
                catch (Exception ex)
                {
                    LogOutgoingGrpcCall(
                        pseudoUrl,
                        request,
                        default(TResponse),
                        ex,
                        stopwatch.ElapsedMilliseconds,
                        headers
                    );

                    throw new O24OpenAPIException("GRPC_ERROR", ex.Message);
                }
            }
        }

        return new AsyncUnaryCall<TResponse>(
            HandleResponseAsync(),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose
        );
    }

    private static void LogOutgoingGrpcCall<TRequest, TResponse>(
        string method,
        TRequest request,
        TResponse? response,
        Exception? exception,
        long duration,
        Metadata headers
    )
    {
        Dictionary<string, string> headerDict = headers.ToDictionary(h => h.Key, h => h.Value);

        ILogger logger = Log.ForContext("LogType", LogType.Grpc)
            .ForContext("Direction", LogDirection.Out)
            .ForContext("Action", method)
            .ForContext("Request", TryPrettifyJson(request))
            .ForContext("Response", TryPrettifyJson(response))
            .ForContext("Error", exception)
            .ForContext("Duration", duration)
            .ForContext("Headers", headerDict.WriteIndentedJson())
            .ForContext("Flow", headerDict.TryGetValue("flow", out string? flow) ? flow : null);
        if (exception is not null)
        {
            logger.Error(exception, exception.Message);
        }
        else
        {
            logger.Information("Outgoing gRPC Call");
        }
    }

    private static string? TryPrettifyJson(object? data)
    {
        if (data is null)
            return null;

        try
        {
            return data.WriteIndentedJson();
        }
        catch
        {
            return data.ToString();
        }
    }
}
