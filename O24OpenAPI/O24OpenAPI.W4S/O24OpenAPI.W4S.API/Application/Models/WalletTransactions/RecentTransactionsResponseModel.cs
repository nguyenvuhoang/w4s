namespace O24OpenAPI.W4S.API.Application.Models.WalletTransactions
{
    public class RecentTransactionsResponseModel : BaseO24OpenAPIModel
    {
        public List<RecentTransactionItem> RecentTransactions { get; set; } = [];
    }

    public class RecentTransactionItem
    {
        public string TransactionId { get; set; } = default!;
        public string Type { get; set; } = default!; // EXPENSE / INCOME
        public int CategoryId { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!;
        public string OccurredAt { get; set; } = default!; // ISO8601 +07:00
        public string Icon { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string Title { get; set; } = default!;
    }

}
