using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletAccount : BaseEntity
{
    public string? AccountNumber { get; set; }
    public int WalletId { get; set; }
    public string? AccountType { get; set; }
    public string? CurrencyCode { get; set; }
    public bool IsPrimary { get; set; }
    public string? Status { get; set; }
}
