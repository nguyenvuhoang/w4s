using Grpc.Core;

namespace O24OpenAPI.GrpcContracts.Factory;

public interface IGrpcClientFactory
{
    TClient GetClient<TClient>()
        where TClient : ClientBase;

    Task<TClient> GetClientAsync<TClient>()
        where TClient : ClientBase;

    Task<AsyncServerStreamingCall<TResponse>> GetServerStreamAsync<
        TClient,
        TRequest,
        TResponse
    >(
        Func<TClient, TRequest, CallOptions, AsyncServerStreamingCall<TResponse>> streamMethod,
        TRequest request,
        CallOptions? callOptions = null
    ) where TClient : ClientBase;
}
