using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletCategoryResponseModel : BaseO24OpenAPIModel
    {
        public WalletCategoryResponseModel() { }
        public string? CategoryId { get; set; }
        public string? WalletId { get; set; }
        public string ParentCategoryId { get; set; } = string.Empty;
        public string? CategoryGroup { get; set; }
        public string? CategoryType { get; set; }
        public string? CategoryName { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public WalletCategoryResponseModel(WalletCategory category)
        {
            CategoryId = category.CategoryId;
            WalletId = category.WalletId;
            ParentCategoryId = category.ParentCategoryId;
            CategoryGroup = category.CategoryGroup;
            CategoryType = category.CategoryType;
            CategoryName = category.CategoryName;
            Icon = category.Icon;
            Color = category.Color;
        }
    }
}
