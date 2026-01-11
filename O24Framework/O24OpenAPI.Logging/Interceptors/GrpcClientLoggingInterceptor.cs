using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Logging.Enums;
using O24OpenAPI.Logging.Helpers;
using Serilog;

namespace O24OpenAPI.Logging.Interceptors;

/// <summary>
/// A gRPC interceptor for logging outgoing calls on the client side.
/// </summary>
public class GrpcClientLoggingInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation
    )
    {
        string correlationIdProp = EngineContext.Current.Resolve<WorkContext>().ExecutionLogId;
        var correlationId = correlationIdProp ?? Guid.NewGuid().ToString();
        var existingHeaders = context.Options.Headers ?? new Metadata();

        // existingHeaders.Add("x-correlation-id", correlationId);

        var newOptions = context.Options.WithHeaders(existingHeaders);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            newOptions
        );

        using (
            Serilog.Context.LogContext.PushProperty(
                LogContextHelper.CorrelationIdKey,
                correlationId
            )
        )
        {
            var stopwatch = Stopwatch.StartNew();
            Exception? exception = null;
            TResponse? response = null;

            var call = continuation(request, newContext);

            var responseTask = call
                .ResponseAsync.ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        exception = task.Exception.GetBaseException();
                    }
                    else
                    {
                        response = task.Result;
                    }
                    return task;
                })
                .Unwrap();

            var dictionaryHeaders = existingHeaders.ToDictionary(s => s.Key, s => s.Value);
            responseTask.ContinueWith(_ =>
            {
                stopwatch.Stop();
                LogOutgoingGrpcCall(
                    context.Method.FullName,
                    request,
                    response,
                    exception,
                    stopwatch.ElapsedMilliseconds,
                    dictionaryHeaders
                );
            });

            return new AsyncUnaryCall<TResponse>(
                responseTask,
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose
            );
        }
    }

    private static void LogOutgoingGrpcCall<TRequest, TResponse>(
        string method,
        TRequest request,
        TResponse? response,
        Exception? exception,
        long duration,
        IDictionary<string, string>? headers
    )
    {
        Log.ForContext("LogType", LogType.Grpc)
            .ForContext("Direction", LogDirection.Out)
            .ForContext("Action", method)
            .ForContext("Request", TryPrettifyJson(request))
            .ForContext("Response", TryPrettifyJson(response))
            .ForContext("Error", exception)
            .ForContext("Duration", duration)
            .ForContext("Headers", JsonConvert.SerializeObject(headers, Formatting.Indented))
            .ForContext(
                "Flow",
                headers is not null && headers.TryGetValue("flow", out var flowValue)
                    ? flowValue.ToString()
                    : null
            )
            .Information("Outgoing gRPC Call Log");
    }

    private static string? TryPrettifyJson(object? data)
    {
        if (data is null)
        {
            return null;
        }

        try
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        catch
        {
            return data.ToString();
        }
    }
}
