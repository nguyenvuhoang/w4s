using LinKit.Core.Cqrs;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Grpc.NCH;

namespace O24OpenAPI.GrpcContracts.Models.NCHModels;

[GrpcClient(typeof(NCHGrpcService.NCHGrpcServiceClient), "CreateZaloOATokenAsync")]
public class CreateZaloOATokenCommand : ICommand<bool>
{
    public string? OaId { get; set; } = default!;
    public string? AppId { get; set; } = default!;
    public string? AccessToken { get; set; } = default!;
    public string? RefreshToken { get; set; } = default!;
    public string? ExpiresIn { get; set; } = default!;
}
