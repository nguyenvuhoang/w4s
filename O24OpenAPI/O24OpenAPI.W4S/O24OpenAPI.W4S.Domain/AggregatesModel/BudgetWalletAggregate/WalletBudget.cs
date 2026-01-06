using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletBudget : BaseEntity
{
    public string? BudgetId { get; set; }
    public string? WalletId { get; set; }
    public string? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? SourceBudget { get; set; }
    public string? SouceTracker { get; set; }
    public string? PeriodType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public static WalletBudget Create(
        string budgetId,
        string walletId,
        string categoryId,
        decimal amount,
        string sourceBudget,
        string? sourceTracker,
        string periodType,
        DateTime startDate,
        DateTime endDate
    )
    {
        return new WalletBudget
        {
            BudgetId = budgetId.ToString(),
            WalletId = walletId,
            CategoryId = categoryId,
            Amount = amount,
            SourceBudget = sourceBudget,
            SouceTracker = sourceTracker,
            PeriodType = periodType,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}
