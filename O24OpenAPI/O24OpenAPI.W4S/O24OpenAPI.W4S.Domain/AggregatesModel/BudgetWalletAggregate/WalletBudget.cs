using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public partial class WalletBudget : BaseEntity
{
    public string? BuggetId { get; set; }
    public string? WalletId { get; set; }
    public string? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? SourceBudget { get; set; }
    public string? SouceTracker { get; set; }
    public string? PeriodType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
