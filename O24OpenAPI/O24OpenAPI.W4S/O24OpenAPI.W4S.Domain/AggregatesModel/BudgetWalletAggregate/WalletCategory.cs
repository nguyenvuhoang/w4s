using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletCategory : BaseEntity
{
    public string? CategoryCode { get; set; }
    public int WalletId { get; set; }
    public int ParentCategoryId { get; set; } = 0;
    public string? CategoryGroup { get; set; }
    public string? CategoryType { get; set; }
    public string? CategoryName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

    // ===== Factory =====
    public static WalletCategory Create(
        string categoryCode,
        int walletId,
        int parentCategoryId,
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

        if (parentCategoryId == 0)
            throw new ArgumentException("ParentCategoryCode is required.");

        return new WalletCategory
        {
            CategoryCode = categoryCode.Trim(),
            WalletId = walletId,
            ParentCategoryId = parentCategoryId,
            CategoryGroup = categoryGroup?.Trim(),
            CategoryType = categoryType?.Trim(),
            CategoryName = categoryName?.Trim(),
            Icon = icon?.Trim(),
            Color = color?.Trim()
        };
    }
}
