using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletProfiles;

[ApiEndpoint(ApiMethod.Post, "wallet-profile", MediatorKey = "w4s")]
public class GetByWalletId : ICommand<WalletProfile>
{
    public string WalletId { get; set; }
}

[CqrsHandler]
public class GetByWalletIdHandler(IWalletProfileRepository walletProfileRepository)
    : ICommandHandler<GetByWalletId, WalletProfile>
{
    public async Task<WalletProfile> HandleAsync(
        GetByWalletId request,
        CancellationToken cancellationToken = default
    )
    {
        return await walletProfileRepository.GetByWalletIdAsync(request.WalletId);
    }
}
