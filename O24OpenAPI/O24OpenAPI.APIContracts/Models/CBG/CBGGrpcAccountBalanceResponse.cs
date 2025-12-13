namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGGrpcAccountBalanceResponse
{
    public string? Currency { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountType { get; set; }
    public decimal? AvailableBalance { get; set; }
    public string? CustomerName { get; set; }
    public decimal? Balance { get; set; }
    public string? Status { get; set; }
}
