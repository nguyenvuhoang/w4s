using Grpc.Core;
using O24OpenAPI.APIContracts.Models.PMT;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.PMT;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.PMT;

public class PMTGrpcClientService : BaseGrpcClientService, IPMTGrpcClientService
{
    private readonly IGrpcClientFactory _grpcClientFactory;
    private readonly Metadata _defaultHeader;

    public PMTGrpcClientService(IGrpcClientFactory grpcClientFactory)
    {
        ServerId = "PMT";
        _grpcClientFactory = grpcClientFactory;
        _defaultHeader = new Metadata()
        {
            {
                "flow",
                $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}"
            },
        };
    }
    public async Task<VNPayProcessPayResponseModel> VNPayProcessPay(string amount, string transactionDescription, string languageCode, string vNPayTransactionDate)
    {
        var request = new VNPayProcessPayRequest
        {
            Amount = amount,
            Transactiondescription = transactionDescription,
            Languagecode = languageCode,
            Vnpaytransactiondate = vNPayTransactionDate
        };
        var pmtGrpcClient =
            await _grpcClientFactory.GetClientAsync<PMTGrpcService.PMTGrpcServiceClient>();
        return await pmtGrpcClient
            .VNPayProcessPayAsync(request, _defaultHeader)
            .CallAsync<VNPayProcessPayResponseModel>();
    }

    public async Task<VNPayProcessReturnModel> VNPayProcessReturn(string rawQuery)
    {
        var request = new VNPayProcessReturnRequest
        {
            Rawquery = rawQuery
        };
        var pmtGrpcClient =
            await _grpcClientFactory.GetClientAsync<PMTGrpcService.PMTGrpcServiceClient>();
        return await cmsGrpcClient
            .VNPayProcessReturnAsync(request, _defaultHeader)
            .CallAsync<VNPayProcessReturnModel>();
    }
}
