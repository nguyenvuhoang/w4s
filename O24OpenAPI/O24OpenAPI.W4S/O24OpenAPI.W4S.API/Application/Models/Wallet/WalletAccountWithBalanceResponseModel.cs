namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletAccountWithBalanceResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public int WalletId { get; set; } = default!; // nvarchar(50) in DB
    public string AccountNumber { get; set; } = default!;
    public string AccountType { get; set; } = default!;
    public string? AccountTypeCaption { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public bool IsPrimary { get; set; }
    public string Status { get; set; } = default!;
    public string? StatusCaption { get; set; }

    public WalletBalanceResponseModel? Balance { get; set; }
}
