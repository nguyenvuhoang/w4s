using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletBugets;

public class GetWalletBudgetsByWalletId
    : BaseTransactionModel,
        ICommand<PagedListModel<WalletBudget, GetWalletBudgetsByWalletModel>>
{
    public int WalletId { get; set; }
}

public class GetWalletBudgetsByWalletModel : BaseO24OpenAPIModel
{
    public string BuggetId { get; set; }
    public int WalletId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string SourceBudget { get; set; }
    public string SouceTracker { get; set; }
    public string PeriodType { get; set; }
}

[CqrsHandler]
public class GetWalletBudgetsByWalletIdHandler(IWalletBudgetRepository walletBudgetRepository)
    : ICommandHandler<
        GetWalletBudgetsByWalletId,
        PagedListModel<WalletBudget, GetWalletBudgetsByWalletModel>
    >
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_WALLET_BUDGETS)]
    public async Task<PagedListModel<WalletBudget, GetWalletBudgetsByWalletModel>> HandleAsync(
        GetWalletBudgetsByWalletId request,
        CancellationToken cancellationToken = default
    )
    {
        IPagedList<WalletBudget> pageList = await walletBudgetRepository.GetAllPaged(q =>
            q.Where(s => s.WalletId == request.WalletId)
        );
        return pageList.ToPagedListModel<WalletBudget, GetWalletBudgetsByWalletModel>();
    }
}
