namespace O24OpenAPI.W4S.API.Application.Models.WalletCategorys
{

    public class TopSpendingCategoriesResponseModel : BaseO24OpenAPIModel
    {
        public List<TopSpendingCategoryItem> TopCategories { get; set; } = [];
    }

    public class TopSpendingCategoryItem
    {
        public string TransactionNumber { get; set; }
        public int CategoryId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Icon { get; set; } = default!;
        public string Color { get; set; } = default!;
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
    }
}
