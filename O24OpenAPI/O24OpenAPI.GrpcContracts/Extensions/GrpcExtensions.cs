using System.Runtime.CompilerServices;
using Grpc.Core;
using LinKit.Json.Runtime;
using O24OpenAPI.Core;
using O24OpenAPI.Grpc.Common;

namespace O24OpenAPI.GrpcContracts.Extensions;

public static class GrpcExtensions
{
    public static async Task<TResult> CallAsync<TResult>(
        this AsyncUnaryCall<GrpcResponse> grpcCall,
        [CallerMemberName] string? operation = null
    )
    {
        var response =
            await grpcCall
            ?? throw new O24OpenAPIException(
                "gRPC call returned no response.",
                "GRPC_NULL_RESPONSE"
            );

        if (response.Code != GrpcResponseCode.Success)
        {
            throw new O24OpenAPIException(
                response.ErrorCode ?? "GRPC_BUSINESS_ERROR",
                $"gRPC call [{operation}] failed with message '{response.Message}'."
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

        var result = response.Data.FromJson<TResult>();
        if (result is null)
        {
            throw new O24OpenAPIException(
                "GRPC_DESERIALIZATION_ERROR",
                $"Failed to deserialize gRPC response data: {response.Data}."
            );
        }

        return result;
    }

    public static async Task<GrpcResponse> GetGrpcResponseAsync<T>(this Task<T> task)
    {
        try
        {
            var result = await task;
            if (result is null)
            {
                return new GrpcResponse
                {
                    Code = GrpcResponseCode.Fail,
                    Message = "Not found.",
                    Data = "",
                };
            }

            return new GrpcResponse
            {
                Code = GrpcResponseCode.Success,
                Message = "OK",
                Data = result is string json ? json : result?.ToJson() ?? "",
            };
        }
        catch (Exception ex)
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
    }

    public static async Task<GrpcResponse> GetGrpcResponseAsync<T>(this object? @object)
    {
        try
        {
            if (@object is Task<T> task)
            {
                @object = await task;
            }

            return new GrpcResponse
            {
                Code = GrpcResponseCode.Success,
                Message = "OK",
                Data = @object is string json ? json : @object?.ToJson() ?? "",
            };
        }
        catch (Exception ex)
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
    }
}
