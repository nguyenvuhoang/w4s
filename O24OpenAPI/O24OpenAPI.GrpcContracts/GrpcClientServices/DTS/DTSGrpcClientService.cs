using Grpc.Core;
using O24OpenAPI.APIContracts.Models.NCH;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.DTS;
using O24OpenAPI.GrpcContracts.Factory;
using O24OpenAPI.GrpcContracts.GrpcClient;
namespace O24OpenAPI.GrpcContracts.GrpcClientServices.DTS;

public class DTSGrpcClientService : BaseGrpcClientService, IDTSGrpcClientService
{
    public DTSGrpcClientService()
    {
        ServerId = "DTS";
    }

    private readonly IGrpcClient<DTSGrpcService.DTSGrpcServiceClient> _dtsGrpcClient =
        EngineContext.Current.ResolveRequired<IGrpcClient<DTSGrpcService.DTSGrpcServiceClient>>();

    public async Task<bool> VerifyUserAysnc(string contractNumber, string idCard)
    {
        var request = new IsExistingContractRequest
        {
            ContractNumber = contractNumber,
            IdCard = idCard,
        };
        return await InvokeAsync<bool>(
            async (header) => await _dtsGrpcClient.Client.IsExistingContractAsync(request, header)
        );
    }

    public async Task<string> GetContractNumberByAccountNumberAsync(string accountNumber)
    {
        var request = new GetContractNumberByAccountNumberRequest { AccountNumber = accountNumber };
        return await InvokeAsync<string>(
            async (header) =>
                await _dtsGrpcClient.Client.GetContractNumberByAccountNumberAsync(request, header)
        );
    }

    public async Task<bool> VerifyUserAnotherDeviceAsync(
        string contractNumber,
        DateTime dob,
        string licenseType,
        string licenseId,
        string language
    )
    {
        var request = new VerifyUserAnotherDeviceRequest
        {
            ContractNumber = contractNumber,
            Dob = dob.ToString("o"),
            LicenseType = licenseType,
            LicenseId = licenseId,
            Language = language,
        };
        return await InvokeAsync<bool>(
            async (header) =>
                await _dtsGrpcClient.Client.VerifyUserAnotherDeviceAsync(request, header)
        );
    }

    /// <summary>
    /// Get SMS Loan Alert by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>

    public async Task<List<SMSLoanAlertModel>> GetSMSLoanAlertAsync(string type)
    {
        var request = new GetSMSLoanAlertRequest
        {
            Type = type
        };
        var grpcFactory = EngineContext.Current.ResolveRequired<IGrpcClientFactory>();

        var streamClient = grpcFactory.GetServerStreamAsync<
            DTSGrpcService.DTSGrpcServiceClient,
            GetSMSLoanAlertRequest,
            GetSMSLoanAlertReply>((client, request, option) => client.GetSMSLoanAlert(request, option), request);

        using var streamingCall = await streamClient;
        var response = new List<SMSLoanAlertModel>();
        await foreach (var reply in streamingCall.ResponseStream.ReadAllAsync())
        {
            response.Add(reply.ToSMSLoanAlertModel()!);
        }
        return response;
    }
}
