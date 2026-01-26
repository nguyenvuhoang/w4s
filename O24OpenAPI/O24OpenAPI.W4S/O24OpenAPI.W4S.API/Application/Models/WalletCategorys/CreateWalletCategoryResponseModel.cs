namespace O24OpenAPI.W4S.API.Application.Models.WalletCategorys
{
    public class CreateWalletCategoryResponseModel : BaseO24OpenAPIModel
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
    }
}
