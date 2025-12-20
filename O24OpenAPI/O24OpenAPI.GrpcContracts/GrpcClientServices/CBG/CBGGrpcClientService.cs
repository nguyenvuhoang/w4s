using LinKit.Json.Runtime;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CBG;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.CBG;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CBG;

public class CBGGrpcClientService : BaseGrpcClientService, ICBGGrpcClientService
{
    public CBGGrpcClientService()
    {
        ServerId = "CBG";
    }

    private readonly IGrpcClient<CBGGrpcService.CBGGrpcServiceClient> _cbgGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<CBGGrpcService.CBGGrpcServiceClient>>();

    public async Task<string> CreateStaticTokenAsync(string identifier)
    {
        var request = new CreateStaticTokenRequest { Identifier = identifier };
        return await InvokeAsync<string>(
            async (header) => await _cbgGrpcClient.Client.CreateStaticTokenAsync(request, header)
        );
    }

    public async Task<CBGGrpcAccountBalanceResponse> HasAccountAsync(string account)
    {
        var request = new AccountRequest { Account = account };
        return await InvokeAsync<CBGGrpcAccountBalanceResponse>(
            async (header) => await _cbgGrpcClient.Client.HasAccountAsync(request, header)
        );
    }

    public async Task<CBGGrpcAccountResponse> DepositAccountIsExistingAsync(string account)
    {
        var request = new AccountRequest { Account = account };
        return await InvokeAsync<CBGGrpcAccountResponse>(
            async (header) =>
                await _cbgGrpcClient.Client.DepositAccountIsExistingAsync(request, header)
        );
    }

    public async Task<string> GetCommandListAsync(string tokenId)
    {
        var request = new TokenRequest { TokenId = tokenId };
        return await InvokeAsync<string>(
            async (header) => await _cbgGrpcClient.Client.GetCommandListAsync(request, header)
        );
    }

    public async Task<CBGGrpcUserSessionModel> GetUserSessionAsync(string tokenId)
    {
        var request = new TokenRequest { TokenId = tokenId };
        return await InvokeAsync<CBGGrpcUserSessionModel>(
            async (header) => await _cbgGrpcClient.Client.GetUserSessionAsync(request, header)
        );
    }

    public async Task<JToken> ExecuteRuleFuncAsync(
        string txCode,
        string ruleName,
        Dictionary<string, object> data
    )
    {
        var request = new ExecuteRuleFuncRequest
        {
            TxCode = txCode,
            RuleName = ruleName,
            JsonData = data.ToJson(),
        };
        return await InvokeAsync<JToken>(
            async (header) => await _cbgGrpcClient.Client.ExecuteRuleFuncAsync(request, header)
        );
    }

    public async Task<CBGatewayResponseModel<object>> CreateFOAsync(string jsonModel)
    {
        var request = new CBRequestJsonWrapper { JsonModel = jsonModel };
        return await InvokeAsync<CBGatewayResponseModel<object>>(
            async (header) => await _cbgGrpcClient.Client.CreateFOAsync(request, header)
        );
    }

    public async Task<CBGPageListModel> SimpleSearchAsync(string jsonModel)
    {
        var request = new CBRequestJsonWrapper { JsonModel = jsonModel };
        return await InvokeAsync<CBGPageListModel>(
            async (header) => await _cbgGrpcClient.Client.SimpleSearchAsync(request, header)
        );
    }

    public async Task<JToken> SearchSqlAsync(string sql)
    {
        var request = new SqlSearchRequest { Sql = sql };
        return await InvokeAsync<JToken>(
            async (header) => await _cbgGrpcClient.Client.SearchSqlAsync(request, header)
        );
    }

    public async Task<JToken> CreateBOAsync(string jsonModel)
    {
        var request = new CBRequestJsonWrapper { JsonModel = jsonModel };
        return await InvokeAsync<JToken>(
            async (header) => await _cbgGrpcClient.Client.CreateBOAsync(request, header)
        );
    }

    public async Task<JToken> CallFunction(string requestModel)
    {
        var request = new CBRequestJsonWrapper { JsonModel = requestModel };
        return await InvokeAsync<JToken>(
            async (header) => await _cbgGrpcClient.Client.CallFunctionAsync(request, header)
        );
    }

    public async Task<CBGGrpcAccountResponse> LoanAccountIsExistingAsync(string account)
    {
        var request = new AccountRequest { Account = account };
        return await InvokeAsync<CBGGrpcAccountResponse>(
            async (header) =>
                await _cbgGrpcClient.Client.LoanAccountIsExistingAsync(request, header)
        );
    }
}
