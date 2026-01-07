using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletBalance : BaseEntity
{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public decimal BonusBalance { get; set; }
    public decimal LockedBalance { get; set; }
    public decimal AvailableBalance { get; set; }
}
