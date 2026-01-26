using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletGoal : BaseEntity
{
    public int WalletId { get; set; }
    public string? GoalName { get; set; }
    public decimal? TargetAmount { get; set; } = 0;
    public decimal? CurrentAmount { get; set; }
    public DateTime? TargetDate { get; set; }
}
