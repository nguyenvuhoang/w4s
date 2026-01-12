using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletCategoryResponseModel : BaseO24OpenAPIModel
{
    public WalletCategoryResponseModel() { }
    public int Id { get; set; }
    public string CategoryCode { get; set; }
    public int? WalletId { get; set; }
    public int ParentCategoryId { get; set; }
    public string? CategoryGroup { get; set; }
    public string? CategoryType { get; set; }
    public string? CategoryName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public string? WebIcon { get; set; }
    public WalletCategoryResponseModel(WalletCategory category)
    {
        Id = category.Id;
        CategoryCode = category.CategoryCode;
        WalletId = category.WalletId;
        ParentCategoryId = category.ParentCategoryId;
        CategoryGroup = category.CategoryGroup;
        CategoryType = category.CategoryType;
        CategoryName = category.CategoryName;
        Icon = category.Icon;
        WebIcon = IconHelper.ToFaIcon(category.Icon);
        Color = category.Color;
    }
}
