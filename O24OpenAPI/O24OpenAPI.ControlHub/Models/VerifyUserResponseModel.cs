using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Response;

public class VerifyUserResponseModel : BaseO24OpenAPIModel
{
    public bool IsVerified { get; set; }
    public string ContractNumber { get; set; }
    public string UserCode { get; set; }
}
