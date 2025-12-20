using Grpc.Core;
using LinKit.Json.Runtime;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClient;

public class ClientGrpc<T>(IGrpcClientFactory factory) : IGrpcClient<T>
    where T : ClientBase<T>
{
    public T Client { get; } = factory.GetClient<T>();

    public async Task<TClient> GetClientAsync<TClient>()
        where TClient : ClientBase
    {
        return await factory.GetClientAsync<TClient>();
    }

    public Metadata GetCommonMetaData()
    {
        Metadata headers = new()
        {
            { "work_context", EngineContext.Current.Resolve<WorkContext>()?.ToJson() },
        };
        return headers;
    }
}
