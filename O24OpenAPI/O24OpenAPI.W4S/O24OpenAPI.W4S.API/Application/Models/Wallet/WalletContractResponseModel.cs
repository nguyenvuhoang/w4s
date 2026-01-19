namespace O24OpenAPI.W4S.API.Application.Models.Wallet;

public class WalletContractResponseModel : BaseO24OpenAPIModel
{
    public int Id { get; set; }
    public string ContractNumber { get; set; } = default!;
    public int? ContractType { get; set; }
    public string ContractTypeCaption { get; set; }
    public string Status { get; set; }
    public string StatusCaption { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime OpenDateUtc { get; set; }
    public DateTime? CloseDateUtc { get; set; }
}
