using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Grpc.PMT;

namespace O24OpenAPI.GrpcContracts.Models.PMTModels;

[GrpcClient(
    typeof(PMTGrpcService.PMTGrpcServiceClient),
    "VNPayProcessReturnAsync"
)]
[GrpcEndpoint(
    typeof(PMTGrpcService.PMTGrpcServiceBase),
    "VNPayProcessReturn",
    MediatorKey = MediatorKey.PMT
)]
public class VNPayProcessReturnCommand :
    ICommand<VNPayProcessReturnResponseModel>
{
    public string? RawQuery { get; set; }
}

public class VNPayProcessReturnResponseModel : BaseO24OpenAPIModel
{
    public string TransactionStatus { get; set; }
    public string TransactionRef { get; set; } = default!;
    public string TransactionStatusMessage { get; set; } = default!;
    public string ResponseCodeStatus { get; set; } = default!;
    public string ResponseCodeMessage { get; set; } = default!;
    public string TransactionDate { get; set; } = default!;
}
