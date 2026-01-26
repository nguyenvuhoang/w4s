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
    public string SourceBudget { get; set; } = string.Empty;
    public int? SourceTracker { get; set; }
    public string? PeriodType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? IncludeInReport { get; set; }
    public bool? IsAutoRepeat { get; set; }
    public string Note { get; set; } = string.Empty;


    public static WalletBudget Create(
        string budgetCode,
        int walletId,
        int categoryId,
        decimal amount,
        string sourceBudget,
        int? sourceTracker,
        string periodType,
        DateTime startDate,
        DateTime endDate,
        bool? includeInReport = null,
        bool? isAutoRepeat = null,
        string note = ""
    )
    {
        return new WalletBudget
        {
            BudgetCode = budgetCode.Trim(),
            WalletId = walletId,
            CategoryId = categoryId,
            Amount = amount,
            SourceBudget = sourceBudget,
            SourceTracker = sourceTracker,
            PeriodType = periodType,
            StartDate = startDate,
            EndDate = endDate,
            IncludeInReport = includeInReport,
            IsAutoRepeat = isAutoRepeat,
            Note = note
        };
    }
}
