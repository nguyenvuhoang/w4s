namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class CreateWalletResponseModel : BaseO24OpenAPIModel
{
    public CreateWalletResponseModel() { }

    public int WalletId { get; set; }
    public string ContractNumber { get; set; }
    public string UserCode { get; set; }
}
