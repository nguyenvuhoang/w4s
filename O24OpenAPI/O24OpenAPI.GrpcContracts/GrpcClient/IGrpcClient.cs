using Grpc.Core;

namespace O24OpenAPI.GrpcContracts.GrpcClient;

public interface IGrpcClient<T>
    where T : ClientBase<T>
{
    T Client { get; }
    Metadata GetCommonMetaData();
}
