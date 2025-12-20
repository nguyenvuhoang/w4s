using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate
{
    public class WalletCategory : BaseEntity
    {
        public required string CategoryId { get; set; }
        public required string WalletId { get; set; }
        public string ParentCategoryId { get; set; } = string.Empty;
        public string? CategoryGroup { get; set; }
        public required string CategoryType { get; set; }
        public required string CategoryName { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
