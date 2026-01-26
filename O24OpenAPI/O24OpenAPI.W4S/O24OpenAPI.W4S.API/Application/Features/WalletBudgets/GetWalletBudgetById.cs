using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletBudgets;

#region Models
public class GetWalletBudgetById : BaseTransactionModel, IQuery<WalletBudgetResponseModel>
{
    public int BudgetId { get; set; }
}

#endregion

#region Handler
[CqrsHandler]
public class GetWalletBudgetByIdHandler(IWalletBudgetRepository walletBudgetRepository)
    : IQueryHandler<GetWalletBudgetById, WalletBudgetResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_BUDGET_BY_ID)]
    public async Task<WalletBudgetResponseModel> HandleAsync(
        GetWalletBudgetById request,
        CancellationToken cancellationToken = default
    )
    {
        WalletBudget budget = await walletBudgetRepository.GetById(request.BudgetId);
        return budget.ToWalletBudgetResponseModel();
    }
}
#endregion
