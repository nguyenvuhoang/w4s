using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class SyncUserInfoModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
    public string PhoneNumber { get; set; }
}
