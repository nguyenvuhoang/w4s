using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletContractResponseModel : BaseTransactionModel
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; } = default!;
        public int? ContractType { get; set; }
        public string? ContractTypeCaption { get; set; }
        public string? Status { get; set; }
        public string? StatusCaption { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }


    }
}
