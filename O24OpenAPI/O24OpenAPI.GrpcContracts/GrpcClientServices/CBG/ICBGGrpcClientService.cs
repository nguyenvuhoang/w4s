using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CBG;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CBG;

public interface ICBGGrpcClientService
{
    Task<string> CreateStaticTokenAsync(string identifier);
    Task<CBGGrpcAccountBalanceResponse> HasAccountAsync(string account);
    Task<CBGGrpcAccountResponse> DepositAccountIsExistingAsync(string account);
    Task<string> GetCommandListAsync(string tokenId);
    Task<CBGGrpcUserSessionModel> GetUserSessionAsync(string tokenId);
    Task<JToken> ExecuteRuleFuncAsync(string txCode, string ruleName, Dictionary<string, object> data);
    Task<CBGatewayResponseModel<object>> CreateFOAsync(string jsonModel);
    Task<CBGPageListModel> SimpleSearchAsync(string jsonModel);
    Task<JToken> SearchSqlAsync(string sql);
    Task<JToken> CreateBOAsync(string jsonModel);
    Task<JToken> CallFunction(string requestModel);
    Task<CBGGrpcAccountResponse> LoanAccountIsExistingAsync(string account);
}
