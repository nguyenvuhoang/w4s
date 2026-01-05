using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletInformationResponseModel : BaseTransactionModel
    {
        public WalletContractResponseModel? Contract { get; set; }
        public IList<WalletProfileDetailResponseModel> Wallets { get; set; } = [];

    }
}
