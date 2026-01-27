namespace O24OpenAPI.W4S.API.Application.Models.WalletCategorys
{
    public class CreateWalletCategoryResponseModel : BaseO24OpenAPIModel
    {
        public int CreatedCount { get; set; }
        public List<CreateWalletCategoryItemResult> Items { get; set; } = [];
    }
    public class CreateWalletCategoryItemResult
    {
        public int WalletId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; } = default!;
    }
}
