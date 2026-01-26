namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletInformationResponseModel : BaseO24OpenAPIModel
{
    public WalletContractResponseModel Contract { get; set; }
    public IList<WalletProfileDetailResponseModel> Wallets { get; set; } = [];
}
