using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet
{
    public class SimpleSearchWalletCategoryCommand : SimpleSearchModel, ICommand<PagedListModel<WalletCategory, WalletCategoryResponseModel>>
    {
    }

    [CqrsHandler]
    public class SimpleSearchWalletCategoryHandle(
        IWalletCategoryRepository walletCategoryRepository
    ) : ICommandHandler<SimpleSearchWalletCategoryCommand, PagedListModel<WalletCategory, WalletCategoryResponseModel>>
    {
        [WorkflowStep(WorkflowStep.W4S.WF_STEP_W4S_RETRIEVE_WALLET_CATEGORY)]
        public async Task<PagedListModel<WalletCategory, WalletCategoryResponseModel>> HandleAsync(
            SimpleSearchWalletCategoryCommand request,
            CancellationToken cancellationToken = default
        )
        {
            IPagedList<WalletCategory> walletCategory = await walletCategoryRepository.GetAllPaged(
                query =>
                {
                    if (!string.IsNullOrEmpty(request.SearchText))
                    {
                        query = query.Where(c =>
                            c.CategoryGroup.Contains(request.SearchText)
                        );
                    }

                    query = query.OrderBy(c => c.Id);
                    return query;
                },
                0,
                0
            );
            return new PagedListModel<WalletCategory, WalletCategoryResponseModel>(walletCategory);
        }
    }
}