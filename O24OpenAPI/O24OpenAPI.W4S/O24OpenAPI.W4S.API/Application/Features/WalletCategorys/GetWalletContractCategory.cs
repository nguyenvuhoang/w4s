using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletCategorys;

public class GetWalletContractCategoryCommand : BaseTransactionModel, ICommand<List<WalletCategoryResponseModel>>
{
    public string ContractNumber { get; set; }
}

[CqrsHandler]
public class GetWalletContractCategoryCommandHandler(IWalletProfileRepository walletProfileRepository, IWalletCategoryRepository walletCategoryRepository)
    : ICommandHandler<GetWalletContractCategoryCommand, List<WalletCategoryResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_WALLET_CONTRACT_CATEGORY)]
    public async Task<List<WalletCategoryResponseModel>> HandleAsync(
        GetWalletContractCategoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var walletProfile = await walletProfileRepository.GetByContractNumberAsync(request.ContractNumber) ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Validation.WalletContractNotFound,
                request.Language
            );

        var walletCategory = await walletCategoryRepository.GetWalletCategoryByWalletIdAsync(walletProfile.Id) ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Validation.WalletCategoryNotFound,
                request.Language
            );

        return [.. walletCategory.Select(x => x.ToWalletCategoryResponseModel())];
    }
}
