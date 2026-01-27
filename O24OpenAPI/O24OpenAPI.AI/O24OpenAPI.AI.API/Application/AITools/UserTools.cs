using System.ComponentModel;
using System.Text.Json;
using LinKit.Core.Abstractions;
using LinKit.Core.Cqrs;
using O24OpenAPI.GrpcContracts.Models.W4SModels;

namespace O24OpenAPI.AI.API.Application.AITools;

[RegisterService(Lifetime.Scoped)]
public class UserTools(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;

    [Description("Lấy thông tin số dư của người dùng, bao gồm tổng số dư và chi tiết từng ví")]
    public async Task<string> GetUserBalance(string userCode)
    {
        var result = await _mediator.QueryAsync(new GetUserBalanceQuery(userCode));
        return JsonSerializer.Serialize(result);
    }

    [Description(
        "Lấy thông tin chi tiêu của người dùng. Có thể lọc theo category hoặc ví nếu cần."
    )]
    public async Task<string> GetUserSpending(string userCode, DateTime fromDate, DateTime toDate)
    {
        var result = await _mediator.QueryAsync(
            new GetUserSpendingQuery(userCode, fromDate, toDate)
        );
        return JsonSerializer.Serialize(result);
    }
}
