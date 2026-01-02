using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
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

    // ===== Factory =====
    public static WalletCategory Create(
        string categoryId,
        string walletId,
        string parentCategoryId,
        string? categoryGroup,
        string? categoryType,
        string? categoryName,
        string? icon,
        string? color
    )
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("CategoryId is required.");

        if (string.IsNullOrWhiteSpace(walletId))
            throw new ArgumentException("WalletId is required.");

        return new WalletCategory
        {
            CategoryId = categoryId.Trim(),
            WalletId = walletId.Trim(),
            ParentCategoryId = parentCategoryId.Trim(),
            CategoryGroup = categoryGroup?.Trim(),
            CategoryType = categoryType?.Trim(),
            CategoryName = categoryName?.Trim(),
            Icon = icon?.Trim(),
            Color = color?.Trim()
        };
    }
}
