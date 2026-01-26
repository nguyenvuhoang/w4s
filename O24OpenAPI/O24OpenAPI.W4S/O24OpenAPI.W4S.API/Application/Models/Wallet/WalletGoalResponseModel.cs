namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletGoalResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public int WalletId { get; set; } = default!;
    public string GoalName { get; set; } = default!;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime? TargetDate { get; set; }
}
