using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletBudget : BaseEntity
{
    public string? BudgetCode { get; set; }
    public int WalletId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public int SourceBudget { get; set; }
    public string? SouceTracker { get; set; }
    public string? PeriodType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public static WalletBudget Create(
        string budgetCode,
        int walletId,
        int categoryId,
        decimal amount,
        int sourceBudget,
        string? sourceTracker,
        string periodType,
        DateTime startDate,
        DateTime endDate
    )
    {
        return new WalletBudget
        {
            BudgetCode = budgetCode.Trim(),
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
