using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletTransactions
{
    public class SimpleSearchWalletTransactionCommand : SimpleSearchModel, ICommand<PagedListModel<WalletTransaction, WalletTransactionResponseModel>>
    {
    }

    [CqrsHandler]
    public class SimpleSearchWalletTransactionHandle(
        IWalletTransactionRepository walletTransactionRepository
    ) : ICommandHandler<SimpleSearchWalletTransactionCommand, PagedListModel<WalletTransaction, WalletTransactionResponseModel>>
    {
        [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_WALLET_TRANSACTIONS)]
        public async Task<PagedListModel<WalletTransaction, WalletTransactionResponseModel>> HandleAsync(
            SimpleSearchWalletTransactionCommand request,
            CancellationToken cancellationToken = default
        )
        {
            IPagedList<WalletTransaction> walletTransaction = await walletTransactionRepository.GetAllPaged(
                query =>
                {
                    if (!string.IsNullOrEmpty(request.SearchText))
                    {
                        query = query.Where(c =>
                            c.TRANDESC.Contains(request.SearchText)
                            || c.TRANSACTIONID.ToString().Contains(request.SearchText)
                        );
                    }

                    query = query.OrderBy(c => c.Id);
                    return query;
                },
                0,
                0
            );
            return new PagedListModel<WalletTransaction, WalletTransactionResponseModel>(walletTransaction);
        }
    }
}