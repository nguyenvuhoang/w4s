namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletTransactionResponseModel : BaseO24OpenAPIModel
{
    public string TransactionId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
}
