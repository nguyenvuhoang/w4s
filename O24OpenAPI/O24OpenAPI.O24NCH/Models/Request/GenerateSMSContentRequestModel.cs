using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class GenerateSMSContentRequestModel : BaseTransactionModel
{
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
