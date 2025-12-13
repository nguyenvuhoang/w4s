using Grpc.Core;
using Linh.JsonKit.Json;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Enums;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.GrpcContracts.Exceptions;
using Serilog;
using System.Runtime.CompilerServices;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices;

public abstract class BaseGrpcClientService
{
    public string ServerId { get; set; } = string.Empty;

    /// <summary>
    /// Invokes a gRPC call that returns a <see cref="GrpcResponse"/> and parses the JSON result into <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize the response data into.</typeparam>
    /// <param name="grpcCall">A function that executes the gRPC call with headers.</param>
    /// <param name="operation">An optional operation name for logging purposes.</param>
    /// /// <returns>The parsed result of type <typeparamref name="TResult"/>.</returns>
    /// <exception cref="GrpcBusinessException">Thrown when the response code is not SUCCESS.</exception>
    /// <exception cref="RpcException">Thrown when a gRPC-level error occurs.</exception>
    protected async Task<TResult> InvokeAsync<TResult>(
        Func<Metadata, Task<GrpcResponse>> grpcCall,
        [CallerMemberName] string? operation = null
    )
    {
        try
        {
            Metadata headers = new()
            {
                { "work_context", EngineContext.Current.Resolve<WorkContext>()?.ToJson() ?? "{}" },
                {
                    "flow",
                    $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}"
                },
            };

            var response =
                await grpcCall(headers)
                ?? throw new O24OpenAPIException("Response is null", "ERR_NULL_RESPONSE");
            if (response.Code != GrpcResponseCode.Success)
            {
                //_logger.LogWarning("[{Operation}] gRPC returned error: {Code} - {Message} (ErrorCode: {ErrorCode})", operation, response.Code, response.Message, response.ErrorCode);
                throw new O24OpenAPIException(
                    response.Message,
                    response.ErrorCode ?? "ERR_UNKNOWN",
                    response.Detail
                );
            }

            if (string.IsNullOrWhiteSpace(response.Data))
            {
                return default!;
            }

            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)response.Data;
            }

            return response.Data.FromJson<TResult>();
        }
        catch (RpcException rpcEx)
        {
            // Nếu cần log lỗi cụ thể theo mã gRPC
            LogError(rpcEx, operation);
            throw;
        }
        catch (Exception ex)
        {
            LogError(ex, operation);
            throw;
        }
    }

    private void LogError(Exception ex, string? operation)
    {
        var workContext = EngineContext.Current.Resolve<WorkContext>();
        var correlationId = workContext?.ExecutionLogId ?? Guid.NewGuid().ToString();
        var flow = $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}";

        Log.ForContext("LogType", LogType.Grpc)
           .ForContext("Direction", "Out")
           .ForContext("Action", operation ?? "Unknown")
           .ForContext("Error", ex)
           .ForContext("CorrelationId", correlationId)
           .ForContext("Flow", flow)
           .Error(ex, "[gRPC Error] {Operation} failed: {Message}", operation ?? "Unknown", ex.Message);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("==================================================");
        Console.WriteLine($"[gRPC Error] Operation   : {operation}");
        Console.WriteLine($"Exception Type           : {ex.GetType().FullName}");
        Console.WriteLine($"Message                  : {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"InnerException           : {ex.InnerException.GetType().FullName} - {ex.InnerException.Message}");
        }
        Console.WriteLine("StackTrace:");
        Console.WriteLine(ex.StackTrace ?? "(no stack trace)");
        Console.WriteLine($"CorrelationId            : {correlationId}");
        Console.WriteLine($"Flow                     : {flow}");
        Console.WriteLine("==================================================");
        Console.ResetColor();
    }
}
