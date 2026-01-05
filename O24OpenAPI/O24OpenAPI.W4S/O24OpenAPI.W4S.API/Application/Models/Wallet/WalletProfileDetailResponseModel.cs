namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletProfileDetailResponseModel : BaseO24OpenAPIModel
    {
        public int Id { get; set; }
        public Guid WalletId { get; set; }
        public string UserCode { get; set; } = default!;
        public string ContractNumber { get; set; } = default!;
        public string WalletName { get; set; } = default!;
        public string WalletType { get; set; } = default!;
        public string? WalletTypeCaption { get; set; }
        public string DefaultCurrency { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? StatusCaption { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public IList<WalletAccountWithBalanceResponseModel> Accounts { get; set; } = [];
        public IList<WalletCategoryResponseModel> Categories { get; set; } = [];
        public IList<WalletBudgetResponseModel> Budgets { get; set; } = [];
        public IList<WalletGoalResponseModel> Goals { get; set; } = [];
        public IList<WalletTransactionResponseModel> Transactions { get; set; } = [];

    }
}
