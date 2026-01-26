using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.WalletTransactions;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletTransactions;

public class GetWalletTransactionByIdQuery : BaseTransactionModel, IQuery<WalletTransactionModel>
{
    public int TransactionId { get; set; }
}

[CqrsHandler]
public class GetWalletTransactionByIdHandler(
    IWalletTransactionRepository walletTransactionRepository
) : IQueryHandler<GetWalletTransactionByIdQuery, WalletTransactionModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_TRAN_BY_ID)]
    public async Task<WalletTransactionModel> HandleAsync(
        GetWalletTransactionByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var tran = await walletTransactionRepository.GetById(request.TransactionId);
        return tran.ToWalletTransactionModel();
    }
}
