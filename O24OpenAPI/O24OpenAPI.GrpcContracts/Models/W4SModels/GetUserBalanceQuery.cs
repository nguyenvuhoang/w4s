using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Grpc.W4S;

namespace O24OpenAPI.GrpcContracts.Models.W4SModels;

[GrpcClient(typeof(W4SGrpcService.W4SGrpcServiceClient), "GetUserBalanceAsync")]
[GrpcEndpoint(
    typeof(W4SGrpcService.W4SGrpcServiceBase),
    "GetUserBalance",
    MediatorKey = MediatorKey.W4S
)]
public class GetUserBalanceQuery : IQuery<string>
{
    public GetUserBalanceQuery() { }

    public GetUserBalanceQuery(string userCode)
    {
        UserCode = userCode;
    }
    public string? UserCode { get; set; }
}
