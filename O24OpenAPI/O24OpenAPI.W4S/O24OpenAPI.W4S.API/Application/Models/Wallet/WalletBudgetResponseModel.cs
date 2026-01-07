namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletBudgetResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public int BudgetId { get; set; } = default!;
    public int WalletId { get; set; } = default!;
    public int? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? SourceBudget { get; set; }
    public string? SouceTracker { get; set; }
    public string? PeriodType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
