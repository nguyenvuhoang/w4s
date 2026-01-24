using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Models.PMT;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Grpc.PMT;

namespace O24OpenAPI.GrpcContracts.Models.PMTModels;

[GrpcEndpoint(
    typeof(PMTGrpcService.PMTGrpcServiceBase),
    "VNPayProcessPay",
    MediatorKey = MediatorKey.PMT
)]
[GrpcClient(typeof(PMTGrpcService.PMTGrpcServiceClient), "VNPayProcessPayAsync")]
public class VNPayProcessPayCommand : ICommand<VNPayProcessPayResponseModel>
{
    public string? Amount { get; set; }
    public string? TransactionDescription { get; set; } = default!;
    public string? LanguageCode { get; set; } = "vn";
    public string? VNPayTransactionDate { get; set; }
}
