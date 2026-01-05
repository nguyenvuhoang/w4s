using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public partial class WalletGoal : BaseEntity
{
    public string? GoalId { get; set; }
    public string? WalletId { get; set; }
    public string? GoalName { get; set; }
    public decimal? TargetAmount { get; set; } = 0;
    public decimal? CurrentAmount { get; set; }
    public DateTime? TargetDate { get; set; }
}
