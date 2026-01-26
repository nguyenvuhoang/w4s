using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.API.Application.Models.Request;

public class GenerateSMSContentRequestModel : BaseTransactionModel
{
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
