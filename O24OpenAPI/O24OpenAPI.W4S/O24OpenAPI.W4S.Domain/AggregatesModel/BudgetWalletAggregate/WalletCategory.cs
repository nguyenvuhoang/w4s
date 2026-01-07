using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletCategory : BaseEntity
{
    public string? CategoryCode { get; set; }
    public int? WalletId { get; set; }
    public string ParentCategoryCode { get; set; } = string.Empty;
    public string? CategoryGroup { get; set; }
    public string? CategoryType { get; set; }
    public string? CategoryName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

    // ===== Factory =====
    public static WalletCategory Create(
        string categoryCode,
        int walletId,
        string parentCategoryCode,
        string? categoryGroup,
        string? categoryType,
        string? categoryName,
        string? icon,
        string? color
    )
    {
        if (string.IsNullOrWhiteSpace(categoryCode))
            throw new ArgumentException("CategoryCode is required.");

        if (walletId <= 0)
            throw new ArgumentException("WalletId is required.");

        if (string.IsNullOrWhiteSpace(parentCategoryCode))
            throw new ArgumentException("ParentCategoryCode is required.");

        return new WalletCategory
        {
            CategoryCode = categoryCode.Trim(),
            WalletId = walletId,
            ParentCategoryCode = parentCategoryCode.Trim(),
            CategoryGroup = categoryGroup?.Trim(),
            CategoryType = categoryType?.Trim(),
            CategoryName = categoryName?.Trim(),
            Icon = icon?.Trim(),
            Color = color?.Trim()
        };
    }
}
