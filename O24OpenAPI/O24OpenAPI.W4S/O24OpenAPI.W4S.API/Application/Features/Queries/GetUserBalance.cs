using Linh.JsonKit.Json;
using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.GrpcContracts.Models.W4SModels;
using O24OpenAPI.W4S.API.Application.Features.WalletStatements;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Queries;

[CqrsHandler]
public class GetUserBalanceHandler(
    [FromKeyedServices(MediatorKey.W4S)] IMediator mediator,
    IWalletProfileRepository walletProfileRepository
) : IQueryHandler<GetUserBalanceQuery, string>
{
    public async Task<string> HandleAsync(
        GetUserBalanceQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var walletProfile = await walletProfileRepository
            .Table.Where(s => s.UserCode == request.UserCode)
            .FirstOrDefaultAsync(cancellationToken);
        var summary = await mediator.SendAsync(
            new WalletIncomeExpenseSummaryCommand(walletProfile.ContractNumber),
            cancellationToken
        );
        return summary.ToJson();
    }
}
