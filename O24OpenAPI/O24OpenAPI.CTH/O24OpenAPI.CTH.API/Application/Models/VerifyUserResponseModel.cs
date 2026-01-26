using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.CTH.API.Application.Models;

public class VerifyUserResponseModel : BaseO24OpenAPIModel
{
    public bool IsVerified { get; set; }
    public string ContractNumber { get; set; }
    public string UserCode { get; set; }
}
