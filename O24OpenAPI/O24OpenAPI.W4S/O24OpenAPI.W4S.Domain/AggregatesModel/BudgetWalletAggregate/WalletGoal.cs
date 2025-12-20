using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate
{
    public class WalletGoal : BaseEntity
    {
        public required string GoalId { get; set; }
        public required string WalletId { get; set; }
        public required string GoalName { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        public DateTime? TargetDate { get; set; }
    }
}
