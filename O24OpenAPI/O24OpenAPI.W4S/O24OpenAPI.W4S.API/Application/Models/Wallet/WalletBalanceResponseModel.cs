namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletBalanceResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public decimal BonusBalance { get; set; }
    public decimal LockedBalance { get; set; }
    public decimal AvailableBalance { get; set; }
}
