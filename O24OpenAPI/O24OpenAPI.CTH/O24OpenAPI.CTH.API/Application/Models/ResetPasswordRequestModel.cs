using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class ResetPasswordRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string DeviceId { get; set; }
}
