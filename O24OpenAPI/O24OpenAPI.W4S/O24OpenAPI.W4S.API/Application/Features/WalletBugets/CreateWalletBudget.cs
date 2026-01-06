using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletBugets;



public class CreateWalletBudgetCommand : BaseTransactionModel, ICommand<CreateWalletBudgetResponseModel>
{
    public string WalletId { get; set; }
    public string CategoryId { get; set; }
    public decimal Amount { get; set; }

    public string SourceBudget { get; set; } = "DEFAULT_CATEGORY";
    public string? SourceTracker { get; set; }

    public string PeriodType { get; set; } = "MONTH";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}



[CqrsHandler]
public class CreateWalletBudgetHandler(
    IWalletBudgetRepository walletBudgetRepository
) : ICommandHandler<CreateWalletBudgetCommand, CreateWalletBudgetResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_BUDGET)]
    public async Task<CreateWalletBudgetResponseModel> HandleAsync(
        CreateWalletBudgetCommand request,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrWhiteSpace(request.WalletId))
            throw new ArgumentException("WalletId is required");

        if (string.IsNullOrWhiteSpace(request.CategoryId))
            throw new ArgumentException("CategoryId is required");

        var entity = WalletBudget.Create(
            budgetId: Guid.NewGuid().ToString(),
            walletId: request.WalletId.ToString(),
            categoryId: request.CategoryId,
            amount: request.Amount,
            sourceBudget: request.SourceBudget,
            sourceTracker: request.SourceTracker,
            periodType: request.PeriodType,
            startDate: request.StartDate,
            endDate: request.EndDate
        );

        await walletBudgetRepository.InsertAsync(entity);

        return new CreateWalletBudgetResponseModel
        {
            BudgetId = entity.BudgetId
        };
    }
}