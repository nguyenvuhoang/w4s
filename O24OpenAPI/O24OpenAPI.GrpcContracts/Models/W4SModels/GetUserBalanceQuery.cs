using LinKit.Core.Cqrs;

namespace O24OpenAPI.GrpcContracts.Models.W4SModels;

public class GetUserBalanceQuery(string? userCode) : IQuery<object>
{
    public string? UserCode { get; set; } = userCode;
}
