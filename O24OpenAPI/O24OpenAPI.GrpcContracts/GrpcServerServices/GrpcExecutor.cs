using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;

namespace O24OpenAPI.GrpcContracts.GrpcServerServices;

public class GrpcExecutor
{
    /// <summary>
    /// Executes a gRPC server operation with consistent error handling and response formatting.
    /// </summary>
    /// <param name="operation">The actual operation logic to execute.</param>
    /// <param name="operationName">The operation name (used for logging).</param>
    /// <returns>A standardized <see cref="GrpcResponse"/> object.</returns>
    public static async Task<GrpcResponse> ExecuteAsync(
        ServerCallContext context,
        Func<Task<object?>> operation
    )
    {
        try
        {
            IServiceProvider serviceProvider = context.GetHttpContext().RequestServices;
            using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            AsyncScope.Scope = scope;
            Metadata header = context.RequestHeaders;
            string stringWorkContext = header
                .FirstOrDefault(x => x.Key.EndsWithOrdinalIgnoreCase("work_context"))
                ?.Value;
            if (stringWorkContext.HasValue())
            {
                WorkContextTemplate workContext =
                    stringWorkContext.ToObject<WorkContextTemplate>()
                    ?? throw new ArgumentNullException(
                        nameof(stringWorkContext),
                        "Cannot parse work context from request headers."
                    );
                EngineContext.Current.Resolve<WorkContext>().SetWorkContext(workContext);
            }
            object result = await operation();
            //if (result is null)
            //{
            //    throw new Exception("Notfound");
            //}
            return new GrpcResponse
            {
                Code = GrpcResponseCode.Success,
                Message = "OK",
                Detail = "",
                Data = "Notfound",//result is string json ? json : result?.ToJson() ?? "",
                ErrorCode = ""
            };
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "[{Operation}] Server error: {Message}", operationName ?? "Unknown", ex.Message);
            if (ex is O24OpenAPIException o24Ex)
            {
                return new GrpcResponse
                {
                    Code = GrpcResponseCode.Fail,
                    Message = o24Ex.Message,
                    ErrorCode = o24Ex.ErrorCode ?? string.Empty,
                    Detail = o24Ex.ToString(),
                };
            }
            return new GrpcResponse
            {
                Code = GrpcResponseCode.Fail,
                Message = ex.Message,
                ErrorCode = "ERR_INTERNAL",
                Detail = ex.ToString(),
            };
        }
    }
}
