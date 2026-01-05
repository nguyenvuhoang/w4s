namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletBudgetResponseModel : BaseO24OpenAPIModel
    {
        public int Id { get; set; }
        public string BudgetId { get; set; } = default!;
        public string WalletId { get; set; } = default!;
        public string? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string? SourceBudget { get; set; }
        public string? SouceTracker { get; set; }
        public string? PeriodType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
