using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate
{
    public class WalletAccount : BaseEntity
    {
        public required string AccountNumber { get; set; }
        public required string WalletId { get; set; }
        public required string AccountType { get; set; }
        public required string CurrencyCode { get; set; }
        public bool IsPrimary { get; set; }
        public required string Status { get; set; }
    }
}
