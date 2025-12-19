using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class VerifyPasswordModel : BaseTransactionModel
{
    public string Password { get; set; }
    public string UserCode { get; set; }
}
