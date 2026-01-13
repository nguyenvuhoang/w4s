using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletBudgets;

public class CreateWalletBudgetCommand
    : BaseTransactionModel,
        ICommand<CreateWalletBudgetResponseModel>
{
    public int WalletId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }

    /// <summary>
    /// Nguồn gốc của “con số budget” (Amount) đến từ đâu
    /// 
    /// </summary>
    public int SourceBudget { get; set; }
    public string SourceTracker { get; set; }

    public string PeriodType { get; set; } = "MONTH";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? IncludeInReport { get; set; }
    public bool? IsAutoRepeat { get; set; }
    public string Note { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateWalletBudgetHandler(
    IWalletBudgetRepository walletBudgetRepository,
    IWalletCategoryRepository walletCategoryRepository
) : ICommandHandler<CreateWalletBudgetCommand, CreateWalletBudgetResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_BUDGET)]
    public async Task<CreateWalletBudgetResponseModel> HandleAsync(
        CreateWalletBudgetCommand request,
        CancellationToken ct = default
    )
    {
        if (request.WalletId == 0)
            throw new ArgumentException("WalletId is required");

        if (request.CategoryId == 0)
            throw new ArgumentException("CategoryId is required");
        WalletCategory category = await walletCategoryRepository.GetById(request.CategoryId);
        WalletBudget entity = WalletBudget.Create(
            budgetCode: GenerateWalletBudgetCode(category.CategoryCode),
            walletId: request.WalletId,
            categoryId: request.CategoryId,
            amount: request.Amount,
            sourceBudget: request.SourceBudget,
            sourceTracker: request.SourceTracker,
            periodType: request.PeriodType,
            startDate: request.StartDate,
            endDate: request.EndDate,
            includeInReport: request.IncludeInReport ?? true,
            isAutoRepeat: request.IsAutoRepeat ?? false,
            note: request.Note
        );

        await walletBudgetRepository.InsertAsync(entity);

        return new CreateWalletBudgetResponseModel { BudgetId = entity.Id };
    }

    private static string GenerateWalletBudgetCode(string categoryCode)
    {
        string date = DateTime.UtcNow.ToString("yyyyMMdd");
        return $"{date}{categoryCode}";
    }
}
