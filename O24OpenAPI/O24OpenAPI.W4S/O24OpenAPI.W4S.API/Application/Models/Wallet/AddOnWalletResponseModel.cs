using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class AddOnWalletResponseModel : BaseO24OpenAPIModel
    {
        public int WalletId { get; set; }
        public string ContractNumber { get; set; }
        public List<WalletLedgerEntry> WalletLedgerEntries { get; set; }
    }
}
