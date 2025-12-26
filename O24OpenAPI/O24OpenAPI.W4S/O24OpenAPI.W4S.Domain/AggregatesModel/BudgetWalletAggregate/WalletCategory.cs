using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public partial class WalletCategory : BaseEntity
{
    public string? CategoryId { get; set; }
    public string? WalletId { get; set; }
    public string ParentCategoryId { get; set; } = string.Empty;
    public string? CategoryGroup { get; set; }
    public string? CategoryType { get; set; }
    public string? CategoryName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
}
