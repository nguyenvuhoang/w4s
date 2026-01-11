using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.WalletTransactions;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletTransactions;

public class GetWalletTransactionByTransactionId
    : BaseTransactionModel,
        IQuery<WalletTransactionModel>
{
    public string TransactionId { get; set; }
}

[CqrsHandler]
public class GetWalletBudgetByTransactionIdHandler(
    IWalletTransactionRepository walletTransactionRepository
) : IQueryHandler<GetWalletTransactionByTransactionId, WalletTransactionModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_TRAN_BY_TRANSACTIONID)]
    public async Task<WalletTransactionModel> HandleAsync(
        GetWalletTransactionByTransactionId request,
        CancellationToken cancellationToken = default
    )
    {
        var tran =
            await walletTransactionRepository.GetByTransactionIdAsync(
                request.TransactionId,
                cancellationToken
            ) ?? throw await O24Exception.CreateAsync(ResourceCode.Common.NotFound);
        return tran.ToWalletTransactionModel();
    }
}
