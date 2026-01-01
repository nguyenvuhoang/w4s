using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class CreateWalletResponseModel : BaseO24OpenAPIModel
    {
        public CreateWalletResponseModel() { }
        public Guid WalletId { get; set; }
        public string ContractNumber { get; set; }
        public string UserCode { get; set; }

    }
}
